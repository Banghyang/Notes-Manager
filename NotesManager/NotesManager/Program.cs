using System;
using System.Text;
using System.Text.Json;

namespace NotesManager
{
    internal class Program
    {
        static List<Note> Notes = new List<Note>();
        static void CreateNote()
        {
            Console.WriteLine("Введите название заметки:\n");
            string noteName = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Введите текст заметки:\n");
            List<string> TempNotes = new List<string>();
            while (true) // Цикл для многострочного ввода
            {
                string note = Console.ReadLine();
                if (note == "<Сохранить>")
                {
                    Console.Clear();
                    Notes.Add(new Note(noteName, string.Join("\n", TempNotes)));
                    SaveNote();
                    Console.WriteLine("\nЗаметка успешно создана!");
                    Console.WriteLine("\nНажмите любую клавишу для продолжения");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
                else if(note == "<Выйти>")
                {
                    Console.Clear();
                    return;
                }
                TempNotes.Add(note);
            }

        }
        static void SaveNote()
        {
            string NoteFile = JsonSerializer.Serialize(Notes, new JsonSerializerOptions { WriteIndented = true});
            File.WriteAllText("Notes.json", NoteFile, Encoding.UTF8);
        }

        static void LoadNote()
        {
            int noteNum = SelectNoteIndex();
            if (noteNum < 0)
            {
                Console.WriteLine("Ошибка! Неверный ввод.");
                Console.WriteLine("\nНажмите любую клавишу для продолжения");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            Console.Clear();
            Console.WriteLine(Notes[noteNum].Text);
            Console.WriteLine("\nНажмите любую клавишу для продолжения");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        static int SelectNoteIndex()
        {
            if (Notes.Count == 0)
            {
                Console.WriteLine("Ошибка! Отсутствуют заметки!");
                Console.WriteLine("\nНажмите любую клавишу для продолжения");
                Console.ReadKey();
                Console.Clear();
                return -1;
            }

            int count = 1;
            foreach (Note a in Notes)
            {
                Console.WriteLine($"{count}. {a.Title}\n");
                count += 1;
            }
            Console.WriteLine("Введите номер записки");
            int noteNum;

            if (int.TryParse(Console.ReadLine(), out noteNum))
            {
                int index = noteNum - 1;
                if (index >= 0 && index < Notes.Count)
                {
                    return index;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        static void EditNote()
        {
            int noteNum = SelectNoteIndex();
            if (noteNum < 0)
            {
                Console.WriteLine("Ошибка! Неверный ввод.");
                Console.WriteLine("\nНажмите любую клавишу для продолжения");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            Console.Clear();
            Console.WriteLine("\nВведите <Сохранить> чтобы обновить записку или <Выйти> чтобы отменить редактирование: ");
            Console.WriteLine("\nВведите новый текст заметки: ");
            List<string> TempNotes = new List<string>();
            while (true) // Цикл для многострочного ввода
            {
                string note = Console.ReadLine();
                if (note == "<Сохранить>")
                {
                    Notes[noteNum].Text = string.Join("\n", TempNotes);
                    SaveNote();
                    Console.Clear();
                    Console.WriteLine("Запись успешно отредактирована!");
                    Console.WriteLine("\nНажмите любую клавишу для продолжения");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
                else if (note == "<Выйти>")
                {
                    Console.Clear();
                    return;
                }
                TempNotes.Add(note);
            }
        }

        static void ImportNotes()
        {
            if (File.Exists("Notes.json"))
            {
                string json = (File.ReadAllText("Notes.json"));
                Notes = JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();
            }
            else
            {
                Notes = new List<Note>();
            }
        }
        static void DeleteNote()
        {
            int noteNum = SelectNoteIndex();
            if (noteNum < 0)
            {
                Console.WriteLine("Ошибка! Неверный ввод.");
                Console.WriteLine("\nНажмите любую клавишу для продолжения");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            Notes.RemoveAt(noteNum);
            SaveNote();
            Console.Clear();
            Console.WriteLine("Запись успешно удалена!");
            Console.WriteLine("\nНажмите любую клавишу для продолжения");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        static void Main()
        {
            ImportNotes();
            while (true)
            {
                Console.WriteLine(string.Concat(Enumerable.Repeat("_", 61)));
                Console.WriteLine("");
                Console.WriteLine("1 - Создать заметку\n2 - Редактировать заметку\n3 - Загрузить заметку\n4 - Удалить заметку");
                Console.WriteLine(string.Concat(Enumerable.Repeat("_", 61)));
                Console.Write("\nВведите опцию (1-4):");
                int noteInput;
                if (int.TryParse(Console.ReadLine(), out noteInput))
                {
                    switch (noteInput)
                    {
                        case 1: Console.Clear(); CreateNote(); break;

                        case 2: Console.Clear(); EditNote(); break;

                        case 3: Console.Clear(); LoadNote(); break;

                        case 4: Console.Clear(); DeleteNote(); break;

                        case 5: return;

                        default: Console.WriteLine("Неверный ввод."); break;

                    }
                }
                else
                {
                    Console.WriteLine("Ошибка! Введите число от 1 до 5!");
                }


            }
        }
    }
}
