namespace SMSGateway.Managers.Interfaces;

public interface IStartingNumberProvider
{
    public string GetNextStartingNo(int typeId);
}