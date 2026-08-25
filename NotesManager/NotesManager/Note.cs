namespace NotesManager
{
    internal class Note
    {
        public string Title { get; set; }
        public string Text { get; set; }

        public Note() { }

        public Note(string title, string text)
        {
            Title = title;
            Text = text;
        }
    }
}
