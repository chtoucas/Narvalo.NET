// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;
    using System.Collections.Generic;

    using Xunit;

    using static global::My;

    using Assert = Narvalo.AssertExtended;

    // Tests for Either<TLeft, TRight>.
    public static partial class EitherFacts {
        [t("OfLeft() returns lefty.")]
        public static void OfLeft1() {
            var either = Either<Obj, Obj>.OfLeft(new Obj("left"));
            Assert.NotNull(either);
            Assert.True(either.IsLeft);
            Assert.False(either.IsRight);
        }

        [t("OfRight() returns righty.")]
        public static void OfRight1() {
            var either = Either<Obj, Obj>.OfRight(new Obj("left"));
            Assert.NotNull(either);
            Assert.True(either.IsRight);
            Assert.False(either.IsLeft);
        }

        [t("Left returns value if lefty.")]
        public static void Left1() {
            var exp = new Obj("left");
            var either = Either<Obj, Obj>.OfLeft(exp);
            Assert.Same(exp, either.Left);
        }

        [t("Left throws if righty.")]
        public static void Left2() {
            var either = Either<Obj, Obj>.OfRight(new Obj("right"));
            Assert.Throws<InvalidOperationException>(() => either.Left);
        }

        [t("Right returns value if righty.")]
        public static void Right1() {
            var exp = new Obj("right");
            var either = Either<Obj, Obj>.OfRight(exp);
            Assert.Same(exp, either.Right);
        }

        [t("Right throws if lefty.")]
        public static void Right2() {
            var either = Either<Obj, Obj>.OfLeft(new Obj("left"));
            Assert.Throws<InvalidOperationException>(() => either.Right);
        }

        [t("LeftOrNone() returns none if righty.")]
        public static void LeftOrNone1() {
            var either = Either<Obj, Obj>.OfRight(new Obj("right"));

            var result = either.LeftOrNone();
            Assert.True(result.IsNone);
        }

        [t("LeftOrNone() returns some if lefty.")]
        public static void LeftOrNone2() {
            var value = new Obj("left");
            var either = Either<Obj, Obj>.OfLeft(value);
            var exp = Maybe.Of(value);

            var result = either.LeftOrNone();
            Assert.True(result.IsSome);
            Assert.True(result.Contains(value));
        }

        [t("RightOrNone() returns none if lefty.")]
        public static void RightOrNone1() {
            var either = Either<Obj, Obj>.OfLeft(new Obj("left"));
            var result = either.RightOrNone();
            Assert.True(result.IsNone);
        }

        [t("RightOrNone() returns some if righty.")]
        public static void RightOrNone2() {
            var value = new Obj("right");
            var either = Either<string, Obj>.OfRight(value);
            var exp = Maybe.Of(value);

            var result = either.RightOrNone();
            Assert.True(result.IsSome);
            Assert.True(result.Contains(value));
        }

        [t("Swap() returns righty if lefty.")]
        public static void Swap1() {
            var exp = new Obj("left");
            var either = Either<Obj, Obj>.OfLeft(exp);
            var swapped = either.Swap();
            Assert.True(swapped.IsRight);
            Assert.True(swapped.ContainsRight(exp));
        }

        [t("Swap() throws if righty.")]
        public static void Swap2() {
            var either = Either<Obj, Obj>.OfRight(new Obj("right"));
            Assert.Throws<InvalidOperationException>(() => either.Swap());
        }

        [t("SwapUnchecked() returns righty if lefty.")]
        public static void SwapUnchecked1() {
            var exp = new Obj("left");
            var either = Either<Obj, Obj>.OfLeft(exp);
            var swapped = either.SwapUnchecked();
            Assert.True(swapped.IsRight);
            Assert.True(swapped.ContainsRight(exp));
        }

        [t("SwapUnchecked() returns lefty if righty.")]
        public static void SwapUnchecked2() {
            var exp = new Obj("right");
            var either = Either<Obj, Obj>.OfRight(exp);
            var swapped = either.SwapUnchecked();
            Assert.True(swapped.IsLeft);
            Assert.True(swapped.ContainsLeft(exp));
        }

        [t("ToLeft() returns value if lefty.")]
        public static void ToLeft1() {
            var exp = new Obj("left");
            var either = Either<Obj, Obj>.OfLeft(exp);
            var left = either.ToLeft();
            Assert.Equal(exp, left);
        }

        [t("ToLeft() throws if righty.")]
        public static void ToLeft2() {
            var either = Either<Obj, Obj>.OfRight(new Obj("right"));
            Assert.Throws<InvalidCastException>(() => either.ToLeft());
        }

        [t("ToRight() returns value if righty.")]
        public static void ToRight1() {
            var exp = new Obj("right");
            var either = Either<Obj, Obj>.OfRight(exp);
            var right = either.ToRight();
            Assert.Equal(exp, right);
        }

        [t("ToRight() throws if lefty.")]
        public static void ToRight2() {
            var either = Either<Obj, Obj>.OfLeft(new Obj("left"));
            Assert.Throws<InvalidCastException>(() => either.ToRight());
        }

        [t("Casting to TLeft returns value if lefty.")]
        public static void cast1() {
            var exp = new Obj("left");
            var either = Either<Obj, string>.OfLeft(exp);
            Assert.Equal(exp, (Obj)either);
        }

        [t("Casting null to TLeft returns default(TLeft).")]
        public static void cast1a() {
            Either<int, string> either1 = null;
            Assert.Equal(default(int), (int)either1);

            Either<int?, string> either2 = null;
            Assert.Equal(default(int?), (int?)either2);

            Either<Obj, string> either3 = null;
            Assert.Equal(default(Obj), (Obj)either3);
        }

        [t("Casting to TLeft throws if righty.")]
        public static void cast2() {
            var either = Either<string, Obj>.OfRight(new Obj("right"));
            Assert.Throws<InvalidCastException>(() => (string)either);
        }

        [t("Casting to TRight returns value if righty.")]
        public static void cast3() {
            var exp = new Obj("right");
            var either = Either<string, Obj>.OfRight(exp);
            Assert.Equal(exp, (Obj)either);
        }

        [t("Casting null to TRight returns default(TLeft).")]
        public static void cast3a() {
            Either<string, int> either1 = null;
            Assert.Equal(default(int), (int)either1);

            Either<string, int?> either2 = null;
            Assert.Equal(default(int?), (int?)either2);

            Either<string, Obj> either3 = null;
            Assert.Equal(default(Obj), (Obj)either3);
        }

        [t("Casting to TRight throws if lefty.")]
        public static void cast4() {
            var either = Either<Obj, string>.OfLeft(new Obj("left"));
            Assert.Throws<InvalidCastException>(() => (string)either);
        }

        [t("Casting from TLeft returns value if righty.")]
        public static void cast5() {
            var exp = new Obj("left");
            var either = (Either<Obj, string>)exp;
            Assert.NotNull(either);
            Assert.True(either.IsLeft);
            Assert.True(either.ContainsLeft(exp));
        }

        [t("Casting from TRight returns value if righty.")]
        public static void cast6() {
            var exp = new Obj("right");
            var either = (Either<string, Obj>)exp;
            Assert.NotNull(either);
            Assert.True(either.IsRight);
            Assert.True(either.ContainsRight(exp));
        }

        [t("ToString() result contains a string representation of the value if lefty.")]
        public static void ToString1() {
            var value = new Obj("Value");
            var either = Either<Obj, Obj>.OfLeft(value);
            Assert.Contains("Left", either.ToString(), StringComparison.OrdinalIgnoreCase);
            Assert.Contains(value.ToString(), either.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        [t("ToString() if lefty null.")]
        public static void ToString2() {
            var either = Either<Obj, Obj>.OfLeft(null);
            Assert.Contains("Left", either.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        [t("ToString() result contains a string representation of the value if righty.")]
        public static void ToString3() {
            var value = new Obj("Value");
            var either = Either<Obj, Obj>.OfRight(value);
            Assert.Contains("Right", either.ToString(), StringComparison.OrdinalIgnoreCase);
            Assert.Contains(value.ToString(), either.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        [t("ToString() if righty null.")]
        public static void ToString4() {
            var either = Either<Obj, Obj>.OfRight(null);
            Assert.Contains("Right", either.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }

    public static partial class EitherFacts {
        [t("ContainsLeft() returns true if it does contain the left 'value'.")]
        public static void ContainsLeft1() {
            var exp = new Obj("left");
            var either = Either<Obj, string>.OfLeft(exp);
            Assert.True(either.ContainsLeft(exp));
            Assert.True(either.ContainsLeft(exp, EqualityComparer<Obj>.Default));
        }

        [t("ContainsLeft() returns false if it does not contain the left 'value'.")]
        public static void ContainsLeft2() {
            var other = new Obj("other");
            var either = Either<Obj, string>.OfLeft(new Obj("left"));
            Assert.False(either.ContainsLeft(other));
            Assert.False(either.ContainsLeft(other, EqualityComparer<Obj>.Default));
        }

        [t("ContainsLeft() returns false if righty.")]
        public static void ContainsLeft3() {
            var exp = new Obj("left");
            var either = Either<Obj, Obj>.OfRight(exp);
            Assert.False(either.ContainsLeft(exp));
            Assert.False(either.ContainsLeft(exp, EqualityComparer<Obj>.Default));
        }

        [t("ContainsRight() returns true if it does contain the right 'value'.")]
        public static void ContainsRight1() {
            var exp = new Obj("right");
            var either = Either<string, Obj>.OfRight(exp);
            Assert.True(either.ContainsRight(exp));
            Assert.True(either.ContainsRight(exp, EqualityComparer<Obj>.Default));
        }

        [t("ContainsRight() returns false if it does not contain the right 'value'.")]
        public static void ContainsRight2() {
            var other = new Obj("other");
            var either = Either<string, Obj>.OfRight(new Obj("right"));
            Assert.False(either.ContainsRight(other));
            Assert.False(either.ContainsRight(other, EqualityComparer<Obj>.Default));
        }

        [t("ContainsRight() returns false if lefty.")]
        public static void ContainsRight3() {
            var exp = new Obj("right");
            var either = Either<Obj, Obj>.OfLeft(exp);
            Assert.False(either.ContainsRight(exp));
            Assert.False(either.ContainsRight(exp, EqualityComparer<Obj>.Default));
        }

        [t("Match() guards.")]
        public static void Match0() {
            var lefty = Either<Obj, Obj>.OfLeft(new Obj("left"));
            Assert.Throws<ArgumentNullException>("caseLeft", () => lefty.Match(null, _ => 1));
            Assert.DoesNotThrow(() => lefty.Match(_ => 1, null));

            var righty = Either<Obj, Obj>.OfRight(new Obj("right"));
            Assert.Throws<ArgumentNullException>("caseRight", () => righty.Match(_ => 1, null));
            Assert.DoesNotThrow(() => righty.Match(null, _ => 1));
        }

        [t("Match() calls caseLeft not caseRight if lefty.")]
        public static void Match1() {
            var lefty = Either<Obj, Obj>.OfLeft(new Obj("left"));
            var notCalled = true;
            var wasCalled = false;
            Func<Obj, int> caseLeft = _ => { wasCalled = true; return 1; };
            Func<Obj, int> caseRight = _ => { notCalled = false; return 1; };

            lefty.Match(caseLeft, caseRight);

            Assert.True(notCalled);
            Assert.True(wasCalled);
        }

        [t("Match() calls caseRight not caseLeft if righty.")]
        public static void Match2() {
            var righty = Either<Obj, Obj>.OfRight(new Obj("right"));
            var notCalled = true;
            var wasCalled = false;
            Func<Obj, int> caseLeft = _ => { notCalled = false; return 1; };
            Func<Obj, int> caseRight = _ => { wasCalled = true; return 1; };

            righty.Match(caseLeft, caseRight);

            Assert.True(notCalled);
            Assert.True(wasCalled);
        }

        [t("WhenLeft() guards.")]
        public static void WhenLeft0() {
            var lefty = Either<Obj, Obj>.OfLeft(new Obj("left"));
            Assert.Throws<ArgumentNullException>("predicate", () => lefty.WhenLeft(null, _ => { }));
            Assert.Throws<ArgumentNullException>("action", () => lefty.WhenLeft(_ => true, null));

            var righty = Either<Obj, Obj>.OfRight(new Obj("right"));
            Assert.DoesNotThrow(() => righty.WhenLeft(null, _ => { }));
            Assert.DoesNotThrow(() => righty.WhenLeft(_ => true, null));
        }

        [t("WhenLeft() calls action if lefty and predicate is true.")]
        public static void WhenLeft1() {
            var lefty = Either<Obj, Obj>.OfLeft(new Obj("left"));
            var wasCalled = false;
            Action<Obj> action = _ => wasCalled = true;

            lefty.WhenLeft(_ => true, action);

            Assert.True(wasCalled);
        }

        [t("WhenLeft() does not call action if lefty and predicate is false.")]
        public static void WhenLeft2() {
            var lefty = Either<Obj, Obj>.OfLeft(new Obj("left"));
            var notCalled = true;
            Action<Obj> action = _ => notCalled = false;

            lefty.WhenLeft(_ => false, action);

            Assert.True(notCalled);
        }

        [t("WhenLeft() does not call action if righty and predicate is true.")]
        public static void WhenLeft3() {
            var righty = Either<Obj, Obj>.OfRight(new Obj("right"));
            var notCalled = true;
            Action<Obj> action = _ => notCalled = false;

            righty.WhenLeft(_ => true, action);

            Assert.True(notCalled);
        }

        [t("WhenLeft() does not call action if righty and predicate is false.")]
        public static void WhenLeft4() {
            var righty = Either<Obj, Obj>.OfRight(new Obj("right"));
            var notCalled = true;
            Action<Obj> action = _ => notCalled = false;

            righty.WhenLeft(_ => false, action);

            Assert.True(notCalled);
        }

        [t("WhenRight() guards.")]
        public static void WhenRight0() {
            var righty = Either<Obj, Obj>.OfRight(new Obj("right"));
            Assert.Throws<ArgumentNullException>("predicate", () => righty.WhenRight(null, _ => { }));
            Assert.Throws<ArgumentNullException>("action", () => righty.WhenRight(_ => true, null));

            var lefty = Either<Obj, Obj>.OfLeft(new Obj("left"));
            Assert.DoesNotThrow(() => lefty.WhenRight(null, _ => { }));
            Assert.DoesNotThrow(() => lefty.WhenRight(_ => true, null));
        }

        [t("WhenRight() calls action if righty and predicate is true.")]
        public static void WhenRight1() {
            var righty = Either<Obj, Obj>.OfRight(new Obj("right"));
            var wasCalled = false;
            Action<Obj> action = _ => wasCalled = true;

            righty.WhenRight(_ => true, action);

            Assert.True(wasCalled);
        }

        [t("WhenRight() does not call action if righty and predicate is false.")]
        public static void WhenRight2() {
            var righty = Either<Obj, Obj>.OfRight(new Obj("right"));
            var notCalled = true;
            Action<Obj> action = _ => notCalled = false;

            righty.WhenRight(_ => false, action);

            Assert.True(notCalled);
        }

        [t("WhenRight() does not call action if lefty and predicate is true.")]
        public static void WhenRight3() {
            var lefty = Either<Obj, Obj>.OfLeft(new Obj("left"));
            var notCalled = true;
            Action<Obj> action = _ => notCalled = false;

            lefty.WhenRight(_ => true, action);

            Assert.True(notCalled);
        }

        [t("WhenRight() does not call action if lefty and predicate is false.")]
        public static void WhenRight4() {
            var lefty = Either<Obj, Obj>.OfLeft(new Obj("left"));
            var notCalled = true;
            Action<Obj> action = _ => notCalled = false;

            lefty.WhenRight(_ => false, action);

            Assert.True(notCalled);
        }

        [t("Do() guards.")]
        public static void Do0() {
            var lefty = Either<Obj, Obj>.OfLeft(new Obj("left"));
            Assert.Throws<ArgumentNullException>("onLeft", () => lefty.Do(null, _ => { }));
            Assert.DoesNotThrow(() => lefty.Do(_ => { }, null));

            var righty = Either<Obj, Obj>.OfRight(new Obj("right"));
            Assert.Throws<ArgumentNullException>("onRight", () => righty.Do(_ => { }, null));
            Assert.DoesNotThrow(() => righty.Do(null, _ => { }));
        }

        [t("Do() calls onLeft not onRight if lefty.")]
        public static void Do1() {
            var lefty = Either<Obj, Obj>.OfLeft(new Obj("left"));
            var notCalled = true;
            var wasCalled = false;
            Action<Obj> onLeft = _ => wasCalled = true;
            Action<Obj> onRight = _ => notCalled = false;

            lefty.Do(onLeft, onRight);

            Assert.True(notCalled);
            Assert.True(wasCalled);
        }

        [t("Do() calls onRight not onLeft if righty.")]
        public static void Do2() {
            var lefty = Either<Obj, Obj>.OfRight(new Obj("right"));
            var notCalled = true;
            var wasCalled = false;
            Action<Obj> onLeft = _ => notCalled = false;
            Action<Obj> onRight = _ => wasCalled = true;

            lefty.Do(onLeft, onRight);

            Assert.True(notCalled);
            Assert.True(wasCalled);
        }

        [t("OnLeft() guards.")]
        public static void OnLeft0() {
            var lefty = Either<Obj, Obj>.OfLeft(new Obj("left"));
            Assert.Throws<ArgumentNullException>("action", () => lefty.OnLeft(null));

            var righty = Either<Obj, Obj>.OfRight(new Obj("right"));
            Assert.DoesNotThrow(() => righty.OnLeft(null));
        }

        [t("OnLeft() calls action if lefty.")]
        public static void OnLeft1() {
            var lefty = Either<Obj, Obj>.OfLeft(new Obj("left"));
            var wasCalled = false;
            Action<Obj> act = _ => wasCalled = true;

            lefty.OnLeft(act);

            Assert.True(wasCalled);
        }

        [t("OnLeft() does not call action if righty.")]
        public static void OnLeft2() {
            var righty = Either<Obj, Obj>.OfRight(new Obj("right"));
            var notCalled = true;
            Action<Obj> act = _ => notCalled = false;

            righty.OnLeft(act);

            Assert.True(notCalled);
        }

        [t("OnRight() guards.")]
        public static void OnRight0() {
            var lefty = Either<Obj, Obj>.OfLeft(new Obj("left"));
            Assert.DoesNotThrow(() => lefty.OnRight(null));

            var righty = Either<Obj, Obj>.OfRight(new Obj("right"));
            Assert.Throws<ArgumentNullException>("action", () => righty.OnRight(null));
        }

        [t("OnRight() calls action if righty.")]
        public static void OnRight1() {
            var righty = Either<Obj, Obj>.OfRight(new Obj("right"));
            var wasCalled = false;
            Action<Obj> act = _ => wasCalled = true;

            righty.OnRight(act);

            Assert.True(wasCalled);
        }

        [t("OnRight() does not call action if lefty.")]
        public static void OnRight2() {
            var lefty = Either<Obj, Obj>.OfLeft(new Obj("left"));
            var notCalled = true;
            Action<Obj> act = _ => notCalled = false;

            lefty.OnRight(act);

            Assert.True(notCalled);
        }
    }

    // Tests for the monadic methods.
    public static partial class EitherFacts {
        [t("SelectLeft() guards.")]
        public static void SelectLeft0() {
            Either<int, int> nil = null;
            Func<int, int> selector = x => x;
            Assert.Throws<ArgumentNullException>("this", () => nil.SelectLeft(selector));

            Either<int, int> either = Either<int, int>.OfLeft(1);
            Assert.Throws<ArgumentNullException>("selector", () => either.SelectLeft(default(Func<int, int>)));
        }

        [t("SelectRight() guards.")]
        public static void SelectRight0() {
            Either<int, int> nil = null;
            Func<int, int> selector = x => x;
            Assert.Throws<ArgumentNullException>("this", () => nil.SelectRight(selector));

            Either<int, int> either = Either<int, int>.OfLeft(1);
            Assert.Throws<ArgumentNullException>("selector", () => either.SelectRight(default(Func<int, int>)));
        }

        [t("Flatten() returns inner 'orientation' if lefty.")]
        public static void Flatten1() {
            var leftleft = Either<Either<int, int>, int>.OfLeft(Either<int, int>.OfLeft(1));
            var result1 = leftleft.Flatten();
            Assert.True(result1.IsLeft);

            var leftright = Either<Either<int, int>, int>.OfLeft(Either<int, int>.OfRight(1));
            var result2 = leftright.Flatten();
            Assert.True(result2.IsRight);
        }

        [t("Flatten() returns righty if righty.")]
        public static void Flatten2() {
            var right = Either<Either<int, int>, int>.OfRight(1);
            var result = right.Flatten();
            Assert.True(result.IsRight);
        }

        [t("FlattenLeft() returns inner 'orientation' if lefty.")]
        public static void FlattenLeft1() {
            var leftleft = Either<Either<int, int>, int>.OfLeft(Either<int, int>.OfLeft(1));
            var result1 = Either.FlattenLeft(leftleft);
            Assert.True(result1.IsLeft);

            var leftright = Either<Either<int, int>, int>.OfLeft(Either<int, int>.OfRight(1));
            var result2 = Either.FlattenLeft(leftright);
            Assert.True(result2.IsRight);
        }

        [t("FlattenLeft() returns righty if righty.")]
        public static void FlattenLeft2() {
            var right = Either<Either<int, int>, int>.OfRight(1);
            var result = Either.FlattenLeft(right);
            Assert.True(result.IsRight);
        }

        [t("FlattenRight() returns inner 'orientation' if righty.")]
        public static void FlattenRight1() {
            var rightright = Either<int, Either<int, int>>.OfRight(Either<int, int>.OfRight(1));
            var result1 = Either.FlattenRight(rightright);
            Assert.True(result1.IsRight);

            var rightleft = Either<int, Either<int, int>>.OfRight(Either<int, int>.OfLeft(1));
            var result2 = Either.FlattenRight(rightleft);
            Assert.True(result2.IsLeft);
        }

        [t("FlattenRight() returns lefty if lefty.")]
        public static void FlattenRight2() {
            var left = Either<int, Either<int, int>>.OfLeft(1);
            var result = Either.FlattenRight(left);
            Assert.True(result.IsLeft);
        }
    }

#if !NO_INTERNALS_VISIBLE_TO

#endif
}
