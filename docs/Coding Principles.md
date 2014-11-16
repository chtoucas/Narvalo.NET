Coding Principles
=================

Strong Name Key
---------------

Create the key pair

    sn -k Application.snk

Extract the public key

    sn -p Application.snk Application.pk

Extract the public key token

    sn -tp Application.pk > Application.txt

### References

+ [Strong Name Tool](http://msdn.microsoft.com/en-us/library/k5b5tt23.aspx)
