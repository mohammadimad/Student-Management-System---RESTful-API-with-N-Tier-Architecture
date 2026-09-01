namespace StudentAPIBusinessLayer;

public sealed record StudentDto(int Id, string Name, int Age, int Grade);

public sealed record StudentInput(string Name, int Age, int Grade);
