public class Numbers : Singleton<Numbers>
{
    public bool CheckBetweenValues(int _amountToCheck, int _min, int _max)
    {
        return ((_amountToCheck < _max) && (_amountToCheck > _min));
    }

    public bool CheckBetweenValues(float _amountToCheck, float _min, float _max)
    {
        return ((_amountToCheck < _max) && (_amountToCheck > _min));
    }

    public void ClampBetweenValues(ref int _amountToCheck, int _min, int _max)
    {
        if (_amountToCheck < _min)
            _amountToCheck = _min;
        if (_amountToCheck > _max)
            _amountToCheck = _max;
    }

    public void ClampBetweenValues(ref float _amountToCheck, float _min, float _max)
    {
        if (_amountToCheck < _min)
            _amountToCheck = _min;
        if (_amountToCheck > _max)
            _amountToCheck = _max;
    }
}