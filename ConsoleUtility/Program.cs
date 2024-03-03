using Application.StaticServices;
using ConsoleUtility;

HashService hashService = new(System.Reflection.Assembly.GetExecutingAssembly());
Decrypte deneme = new Decrypte(hashService);
