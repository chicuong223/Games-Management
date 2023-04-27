using Models;
using Utils;

List<Genre>? genres = new List<Genre>();
//{
//    new Genre("Visual Novel"),
//    new Genre("Horror"),
//    new Genre("Sport"),
//    new Genre("JRPG"),
//    new Genre("Sandbox"),
//    new Genre("Adventure"),
//    new Genre("Puzzle"),
//    new Genre("Hack & Slash"),
//    new Genre("RPG"),
//    new Genre("Simulator"),
//    new Genre("First Person"),
//    new Genre("Strategy")
//};

//XmlUtils.WriteToXml(genres, Directory.GetCurrentDirectory(), "genres.xml");
genres = (List<Genre>)XmlUtils.ReadFromXml<List<Genre>>(Path.Combine(Directory.GetCurrentDirectory(), "genres.xml"));

foreach (var genre in genres)
{
    Console.WriteLine(genre.Name);
}