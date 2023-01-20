
public interface IInteraction
{
    /// <summary>
    /// Characters can interact with special objects in level. Character type decides the action of the object.
    /// </summary>
    /// <param name="character">Character type</param>
    void interact(CharType character);
}