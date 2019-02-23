# SKN_to_OBJ
DLL exposing functions for converting a League of Legends .SKN model file to a WaveFront .OBJ format. Written in C#. 
Created in 2016, refactoring soon.

## TL;DR:
*The following instructions assume that you have set up a dev environment with a C# compiler like Roslyn
which also supports building projects targetting the .NET framework 4.5.2 (although older versions may work still).
Visual Studio 2017/2019 will be used in the example.*

* Clone repository: `git clone https://github.com/Jynsaillar/SKN_to_OBJ.git`
* Build the solution as `Class Library` using the `Release` build configuration.
* Load your project that needs the functionality of the DLL or create a new project, then reference the `SKN_TO_OBJ.dll` you built in the last step.

## Examples

### Loading a .SKN model and immediately writing it to a .OBJ file:

```cs
using System;

namespace SknToObj
{
    class Program
    {
        static void Main(string[] args)
        {
            // For simplicity, assume that:
            // args[0] contains a path to a valid .SKN (skeleton/skin) model file,
            // args[1] contains the path to the output directory of the .OBJ model file.

            string sknModelSrcPath = args[0];
            string objModelDestPath = args[1];
            string objModelFilename = System.IO.Path.GetFileNameWithoutExtension(sknModelSrcPath);
            var sknParser = new SKN_to_OBJ.SKNParser();
            try
            {
                var sknModel = sknParser.ReadSKN(sknModelSrcPath);
                sknParser.WriteOBJ(fileDir: objModelDestPath, fileName: objModelFilename, sourceSkin: sknModel);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An Exception occurred:\n{ex.Message})");
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}

```
