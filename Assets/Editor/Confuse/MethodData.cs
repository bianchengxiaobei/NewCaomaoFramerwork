using Mono.Cecil;
public class MethodData
{
    public MethodDefinition MethodDefinition;
    private int hashcode;
    public MethodData(MethodDefinition method)
    {
        this.MethodDefinition = method;
        this.hashcode = this.CalculateHash();
    }
    public int CalculateHash()
    {
        return this.MethodDefinition.Name.GetHashCode() ^ this.GetHashCode();
    }
}