TODO
====

http://www.visualstudio.com/downloads/download-visual-studio-vs

I see that Microsoft projects include the following lines in their
assembly infos, what's the purpose?
  [assembly: SatelliteContractVersion("X.X.X.X")]
  [assembly: AssemblyMetadata("Serviceable", "True")]

  [assembly: StringFreezing]
  NO: this disable string interning [assembly: CompilationRelaxations(8)]

  [assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
  [assembly: BitmapSuffixInSatelliteAssembly]
  [assembly: TypeLibVersion(2, 4)]

  [assembly: Dependency("System,", LoadHint.Always)]
  [assembly: DefaultDependency(LoadHint.Always)]
  [assembly: AssemblyDefaultAlias("System.Web.dll")]

  [assembly: SecurityCritical]
  [assembly: SecurityRules(SecurityRuleSet.Level1, SkipVerificationInFullTrust = true)]
  [assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
  [assembly: AllowPartiallyTrustedCallers(PartialTrustVisibilityLevel = PartialTrustVisibilityLevel.NotVisibleByDefault)]

  [assembly: AssemblyTargetedPatchBand("1.0.23-161462647")]
  [assembly: ComCompatibleVersion(1, 0, 3300, 0)]
  [module: UnverifiableCode]

