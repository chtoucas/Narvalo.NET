---
uid: Narvalo.Web.UI.ControlBuilderExtensions
---

Ce code est inspiré de la classe `Omari.Web.UI.ControlBuilderExtensions`.
Par rapport au code d'origine, on a effectué les changements suivants :
- utilisation de méthodes d'extension ;
- certaines propriétés étant maintenant publiques, on a supprimé les méthodes
  permettant d'accéder aux propriétés suivantes :
  * `ComplexPropertyEntries` ;
  * `SubBuilders` ;
  * `TemplatePropertyEntries`.
- on préfère conserver la signature d'origine, ie `ICollection` plutôt que `ArrayList` ;
- désactivation des méthodes dont l'utilité ne saute pas aux yeux (en ce qui nous concerne) :
  * `GetParentBuilder()` qui donne accès à la propriété interne `ParentBuilder` ;
  * `GetSimplePropertyEntries()` qui donne accès à la propriété interne `SimplePropertyEntries` ;
  * `GetRootBuilder()`.

**WARNING:** Cette classe dépend de l'API non publique de `ControlBuilder`.
