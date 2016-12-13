---
uid: Narvalo.Check.Unreachable(System.String)
remarks: Adapted from a [MSDN blog](https://blogs.msdn.microsoft.com/francesco/2014/09/12/how-to-use-cccheck-to-prove-no-case-is-forgotten/).
---

Unfortunately, CCCheck will still complain with _CodeContracts: requires unreachable_.
You can safely suppress this warning and, later on, if you reach the "unreachable"
point, CCCheck will produce a different warning: _This requires, always
leading to an error, may be reachable. Are you missing an enum case?_.

---
uid: Narvalo.Check.Unreachable``1(``0)
remarks: Adapted from a [MSDN blog](https://blogs.msdn.microsoft.com/francesco/2014/09/12/how-to-use-cccheck-to-prove-no-case-is-forgotten/).
---

