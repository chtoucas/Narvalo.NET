
### PowerShell Signing

PS> Set-ExecutionPolicy RemoteSigned

PS> Set-ExecutionPolicy AllSigned
PS> Get-ChildItem cert:\CurrentUser\My -CodeSign
PS> Get-ChildItem -Path cert:\CurrentUser\my -CodeSigningCert

Self-Signed certificate:
Use makecert from the Microsoft SDK (C:\Program Files (x86)\Microsoft SDKs\Windows\v7.1A\Bin\makecert.exe)

Create a local certificate authority for your computer:
```
:: Create a certificate CARoot.cer (public) and a private key file CARoot.pvk (private)
makecert.exe ^
    -n "CN=CARoot" ^
    -r ^
    -pe ^
    -a sha512 ^
    -len 4096 ^
    -cy authority ^
    -sv CARoot.pvk ^
    CARoot.cer

:: Copy the public key and private key into a personal information exchange file CARoot.pfx (private)
pvk2pfx.exe ^
    -pvk CARoot.pvk ^
    -spc CARoot.cer ^
    -pfx CARoot.pfx ^
    -po Test123

makecert.exe ^
    -n "CN=PowerShell Local Certificate Root" ^
    -r ^
    -a sha512 ^
    -pe ^
    -len 4096 ^
    -cy authority ^
    -eku 1.3.6.1.5.5.7.3.3 ^
    -sv CARoot.pvk ^
    -ss Root ^
    -sr LocalMachine ^
    CARoot.cer
```
Details:
- `-n CN=PowerShell Local Certificate Root`, subject's certificate name
  and must be formatted as the standard: "CN=Your CA name here"
- `-r`, indicates that this certificate is self signed
- `-a sha512`, declare which signature algorithm we will be using
- `-pe`, Marks the generated private key as exportable. This allows the private key to be included in the certificate.
- `-len 4096`, the generated key length in bits
- `-cy authority`, specifies that this is a certificate authority
- `-sv CARoot.pvk`, the subject's .pvk private key file
- `CARoot.cer`, the certificate file
Install certificate directly into the Trusted Root CA store
- `-ss Root`
- `-sr LocalMachine`

makecert.exe -r -pe -n CN=CertificateName -ss my -sr localmachine -eku 1.3.6.1.5.5.7.3.2 -len 2048 -e 01/01/2016 CertificateName.cer
Create a personal certificate from the above authority:
PS> makecert -pe -n "CN=PowerShell User" -ss MY -a sha1 -eku 1.3.6.1.5.5.7.3.3 -iv root.pvk -ic root.cer

PS> Set-AuthenticodeSignature [script] @(Get-ChildItem cert:\CurrentUser\My -CodeSign)[0]

References:
- [Makecert](http://msdn.microsoft.com/en-us/library/bfsktky3%28v=vs.110%29.aspx)
- [hanselman](http://www.hanselman.com/blog/SigningPowerShellScripts.aspx)
- http://www.jayway.com/2014/09/03/creating-self-signed-certificates-with-makecert-exe-for-development/
- http://blogs.technet.com/b/heyscriptingguy/archive/2010/06/16/hey-scripting-guy-how-can-i-sign-windows-powershell-scripts-with-an-enterprise-windows-pki-part-1-of-2.aspx
- http://blogs.technet.com/b/heyscriptingguy/archive/2010/06/17/hey-scripting-guy-how-can-i-sign-windows-powershell-scripts-with-an-enterprise-windows-pki-part-2-of-2.aspx

### Custom assembly and module attributes

I see that Microsoft projects include the following lines in their
assembly infos, what's their purposes?
```
  [module: UnverifiableCode]
  [assembly: SatelliteContractVersion("X.X.X.X")]
  [assembly: AssemblyMetadata("Serviceable", "True")]

  [assembly: StringFreezing]
  // Disable string interning.
  [assembly: CompilationRelaxations(8)]

  [assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
  [assembly: BitmapSuffixInSatelliteAssembly]
  [assembly: TypeLibVersion(2, 4)]

  [assembly: Dependency("System,", LoadHint.Always)]
  [assembly: DefaultDependency(LoadHint.Always)]
  [assembly: AssemblyDefaultAlias("System.Web.dll")]

  [assembly: AssemblyTargetedPatchBand("1.0.23-161462647")]
  [assembly: ComCompatibleVersion(1, 0, 3300, 0)]
```

### Security attributes

```
[assembly: SecurityRules(SecurityRuleSet.Level2)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityTransparent]
//[assembly: SecurityCritical]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
[assembly: SecurityRules(SecurityRuleSet.Level1, SkipVerificationInFullTrust = true)]
[assembly: AllowPartiallyTrustedCallers(PartialTrustVisibilityLevel = PartialTrustVisibilityLevel.NotVisibleByDefault)]
```
  by the VS extension. See:
  * https://github.com/nodatime/nodatime/issues/268
  * [CAS](http://msdn.microsoft.com/en-us/library/c5tk9z76%28v=vs.110%29.aspx)
  * [APTCA](http://msdn.microsoft.com/fr-fr/magazine/ee336023.aspx)
  * [SecAnnotate](http://blogs.msdn.com/b/shawnfa/archive/2009/11/18/using-secannotate-to-analyze-your-assemblies-for-transparency-violations-an-example.aspx)
