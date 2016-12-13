---
uid: Narvalo.Mvp.Resolvers.CompositeViewTypeBuilder
---

To support composite views, we dynamically emit a type which
takes multiple views, and exposes them as a single view of
the same interface. It's something like this:

```csharp
public class TestViewComposite : CompositeView<ITestView>, ITestView
{
    public TestViewModel Model
    {
        get { return Views.First().Model; }
        set
        {
            foreach (var view in Views) {
                view.Model = value;
            }
        }
    }

    public event EventHandler Searching
    {
        add
        {
            foreach (var view in Views) {
                view.Searching += value;
            }
        }
        remove
        {
            foreach (var view in Views) {
                view.Searching -= value;
            }
        }
    }
}
```

---
uid: Narvalo.Mvp.Resolvers.CompositeViewTypeBuilder.DefineGetter(PropertyInfo)
---

Produces something functionally equivalent to this:
```csharp
get
{
    return Views.First().Model;
}
```
It does this by emitting IL like this:
```csharp
.method public hidebysig newslot specialname virtual final
    instance class WebFormsMvp.FeatureDemos.Logic.Views.Models.CompositeDemoViewModel
    get_Model() cil managed
{
    // Code size       22 (0x16)
    .maxstack  1
    .locals init ([0] class WebFormsMvp.FeatureDemos.Logic.Views.Models.CompositeDemoViewModel CS$1$0000)
    IL_0000:  nop
    IL_0001:  ldarg.0
    IL_0002:  call       instance class [mscorlib]System.Collections.Generic.IEnumerable`1<!0> class [WebFormsMvp]WebFormsMvp.CompositeView`1<class WebFormsMvp.FeatureDemos.Logic.Views.ICompositeDemoView>::get_Views()
    IL_0007:  call       !!0 [System.Core]System.Linq.Enumerable::First<class WebFormsMvp.FeatureDemos.Logic.Views.ICompositeDemoView>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>)
    IL_000c:  callvirt   instance !0 class [WebFormsMvp]WebFormsMvp.IView`1<class WebFormsMvp.FeatureDemos.Logic.Views.Models.CompositeDemoViewModel>::get_Model()
    IL_0011:  stloc.0
    IL_0012:  br.s       IL_0014
    IL_0014:  ldloc.0
    IL_0015:  ret
} // end of method CompositeDemoViewComposite::get_Model
```

---
uid: Narvalo.Mvp.Resolvers.CompositeViewTypeBuilder.DefineSetter(PropertyInfo)
---

Produces something functionally equivalent to this:
```csharp
set
{
    foreach(var view in Views)
        view.Model = value;
}
```
It does this by emitting IL like this:
```csharp
.method public hidebysig newslot specialname virtual final
    instance void  set_Model(class WebFormsMvp.FeatureDemos.Logic.Views.Models.CompositeDemoViewModel 'value') cil managed
{
    // Code size       61 (0x3d)
    .maxstack  2
    .locals init ([0] class WebFormsMvp.FeatureDemos.Logic.Views.ICompositeDemoView view,
        [1] class [mscorlib]System.Collections.Generic.IEnumerator`1<class WebFormsMvp.FeatureDemos.Logic.Views.ICompositeDemoView> CS$5$0000,
        [2] bool CS$4$0001)
    IL_0000:  nop
    IL_0001:  nop
    IL_0002:  ldarg.0
    IL_0003:  call       instance class [mscorlib]System.Collections.Generic.IEnumerable`1<!0> class [WebFormsMvp]WebFormsMvp.CompositeView`1<class WebFormsMvp.FeatureDemos.Logic.Views.ICompositeDemoView>::get_Views()
    IL_0008:  callvirt   instance class [mscorlib]System.Collections.Generic.IEnumerator`1<!0> class [mscorlib]System.Collections.Generic.IEnumerable`1<class WebFormsMvp.FeatureDemos.Logic.Views.ICompositeDemoView>::GetEnumerator()
    IL_000d:  stloc.1
    .try
    {
        IL_000e:  br.s       IL_001f

        IL_0010:  ldloc.1
        IL_0011:  callvirt   instance !0 class [mscorlib]System.Collections.Generic.IEnumerator`1<class WebFormsMvp.FeatureDemos.Logic.Views.ICompositeDemoView>::get_Current()
        IL_0016:  stloc.0
        IL_0017:  ldloc.0
        IL_0018:  ldarg.1
        IL_0019:  callvirt   instance void class [WebFormsMvp]WebFormsMvp.IView`1<class WebFormsMvp.FeatureDemos.Logic.Views.Models.CompositeDemoViewModel>::set_Model(!0)
        IL_001e:  nop
        IL_001f:  ldloc.1
        IL_0020:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
        IL_0025:  stloc.2
        IL_0026:  ldloc.2
        IL_0027:  brtrue.s   IL_0010

        IL_0029:  leave.s    IL_003b

    }  // end .try
    finally
    {
        IL_002b:  ldloc.1
        IL_002c:  ldnull
        IL_002d:  ceq
        IL_002f:  stloc.2
        IL_0030:  ldloc.2
        IL_0031:  brtrue.s   IL_003a

        IL_0033:  ldloc.1
        IL_0034:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
        IL_0039:  nop
        IL_003a:  endfinally
    }  // end handler
    IL_003b:  nop
    IL_003c:  ret
} // end of method CompositeDemoViewComposite::set_Model
```
