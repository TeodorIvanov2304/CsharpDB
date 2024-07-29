/*
1. I have deleted Boardgames DB , created in MS SQL module! It will be replaced with Boardgames, but the original DB is available. 
 
 2. Use regex validation directly in the dto class:

[Required]
[RegularExpression(@"www.[a-zA-Z0-9-]+.com")]
public string Website { get; set; } = null!;
*/