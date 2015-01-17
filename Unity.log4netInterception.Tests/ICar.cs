namespace Unity.log4netInterception.Tests
{
    public interface ICar
    {
        bool Drive(int speed);
        void DontLogMe();
        void Throw();

        void DoSomethingWithReferenceType(string s);
    }
}