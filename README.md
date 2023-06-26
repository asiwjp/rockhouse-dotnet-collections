RockHouse.Collections
===================

RockHouse.Collections is a Collection class package that mimics the Java language API and the Apache.Commons.CollectionsAPI.
It also adds several proprietary APIs.

The following is an example of a class.
- Dictionaries.HashMap
- Dictionaries.LinkedHashMap
- Dictionaries.LinkedOrderedDictionary
- Dictionaries.ListOrderedDictionary
- Dictionaries.LruMap/Dictionary
- Dictionaries.ReferenceDictionary
- Dictionaries.WeakHashMap
- Dictionaries.Multi.HashSetValuedDictionary
- Dictionaries.Multi.ListValuedMap/Dictionary
- Sets.LinkedHashSet
- Sets.LinkedOrderedSet
- Sets.ListOrderedSet
etc

Java language-like
-------
```
var map = new HashMap<string, string>();

if (map.IsEmpty)
{
    Console.WriteLine("empty");
}

var get1 = map.Get("a"); // get1 is null

var put1 = map.Put("a", "1"); // put1 is null
var get2 = map.Get("a"); // get2 is 1

var put2 = map.Put("a", "2"); // put2 is 1
var get3 = map.Get("a"); // get3 is 2
```

>Note:
>There are some incompatibilities with Java.
>Since null is not allowed in primitive types, the API that returns null substitutes default(V).
>For example, default(int) is 0.

Compatibility with .NET API
-------
### Collection initializer
```
var map = new HashMap<string, int>()
{
    { "a", 1 },
    { "b", 2 },
    { "c", 3 },
};
```

### Indexer
```
map["key"] = 10;
var value = map["key"]; // value is 10
var abort = map["not found"]; // KeyNotFoundException
```

### Enumerable
```
foreach (var entry in map)
{
    Console.WriteLine($"key={entry.Key}, value={entry.Value}");
}
```

### LINQ
```
var filteredList = map.Where(e => e.Value > 1)
                        .Select(e => e.Key)
                        .ToList();
```

### Json

With System.Text.Json
```
var json = JsonSerializer.Serialize(map);
var restore = JsonSerializer.Deserialize<HashMap<string, int>>(json);
```

License
-------
This code is under the [Apache License, Version 2.0](https://opensource.org/license/apache-2-0/).
