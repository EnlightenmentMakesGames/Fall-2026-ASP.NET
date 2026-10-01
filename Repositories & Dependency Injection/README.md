Modification of the existing MicroBlog folder in the repository root.
Supports shifting between using a local "RAM" copy of blog posts and the existing Json blog post file.

To use Json mode, line 10 of Program.cs should be commented, with line 9 uncommented.
New blog posts added in Json mode will be saved in the Json file in /data.

To use RAM mode, line 9 of Program.cs should be commented, with line 10 uncommented.
New blog posts added in RAM mode will be stored until the dotnet server is terminated.

This directory's Program.cs is currently set to use Json mode.
