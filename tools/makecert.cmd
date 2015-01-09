Exit 1

"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.1A\Bin\makecert.exe" ^
-n "CN=CARoot" ^
-r ^
-pe ^
-a sha512 ^
-len 4096 ^
-cy authority ^
-sv CARoot.pvk ^
CARoot.cer

"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.1A\Bin\pvk2pfx.exe" ^
-pvk CARoot.pvk ^
-spc CARoot.cer ^
-pfx CARoot.pfx ^
-po Test123