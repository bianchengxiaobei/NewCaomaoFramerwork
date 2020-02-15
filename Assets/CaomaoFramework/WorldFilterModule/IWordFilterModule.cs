
public interface IWordFilterModule
{
    void Init();
    bool ContainsWord(string text);
    string Replace(string text, char replaceChar = '*');
}