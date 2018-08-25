# Scriptable Build Pipeline Debugging Tools
Collection of useful debugging utilities for those of us crazy enough to stare into the abyss.
Simplest entry point is to use the "Window/Asset Management/Debug Build Window" to trigger builds and edit basic details such as using the cache, cache server, or advanced debugging output. When using advanced debugging output, the output folder will contain all the raw data contained in the built bundles, as well as human readable yaml versions of that data and josn output of the data used to write that data.

Requires a Json.NET dll implementation for Unity.
* Tested: https://github.com/SaladLab/Json.Net.Unity3D
* Untested: https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347
