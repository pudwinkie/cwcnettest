
// �p�������@�~���ĴX�ӬP��
public static int GetWeekNumber(DateTime date)
{
    var ci = CultureInfo.CurrentCulture;
    return ci.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
}

/// <summary>

/// ���o�Y����� 24 �p�ɮɨ�C��

/// </summary>

/// <param name="dt">�Y���</param>

/// <returns>�Y����� 24 �p�ɮɨ�C��</returns>

public static DateTime[] GetTheHoursOfDay(DateTime dt)

{

    List<DateTime> dtList = new List<DateTime>();

 

    for (int i = 0; i < 24; i++)

    {

        dtList.Add(new DateTime(dt.Year, dt.Month, dt.Day, i, 0, 0));

    }

 

    return dtList.ToArray();

}

 

/// <summary>

/// ���o�Y����b�ӬP�����Ĥ@�� (�P����)

/// </summary>

/// <param name="dt">�Y���</param>

/// <returns>�Y����b�ӬP�����Ĥ@�� (�P����)</returns>

public static DateTime GetTheFirstDayOfWeek(DateTime dt)

{

    return dt.AddDays((int) dt.DayOfWeek*-1).Date;

}

 

/// <summary>

/// ���o�Y����b�ӬP�����̫�@�� (�P����)

/// </summary>

/// <param name="dt">�Y���</param>

/// <returns>�Y����b�ӬP�����̫�@�� (�P����)</returns>

public static DateTime GetTheLastDayOfWeek(DateTime dt)

{

    return dt.AddDays(7 + (int) dt.DayOfWeek*-1 - 1).Date;

}

 

/// <summary>

/// ���o�Y����b�Ӥ�����Ĥ@�� (1 ��)

/// </summary>

/// <param name="dt">�Y���</param>

/// <returns>�Y����b�Ӥ�����Ĥ@��</returns>

public static DateTime GetTheFirstDayOfMonth(DateTime dt)

{

    return new DateTime(dt.Year, dt.Month, 1);

}

 

/// <summary>

/// ���o�Y����b�Ӥ�����̫�@��

/// </summary>

/// <param name="dt">�Y���</param>

/// <returns>�Y����b�Ӥ�����̫�@��</returns>

public static DateTime GetTheLastDayOfMonth(DateTime dt)

{

    return new DateTime(dt.Year, dt.Month + 1, 1).AddDays(-1);

}

 

/// <summary>

/// ���o�Y����b�Ӥ���C�g���Ĥ@�ѦC��

/// </summary>

/// <param name="dt">�Y���</param>

/// <returns>�Y����b�Ӥ���C�g���Ĥ@�ѦC��</returns>

public static DateTime[] GetTheFirstDaysOfWeekInMoth(DateTime dt)

{

    List<DateTime> dtList = new List<DateTime>();

 

    DateTime dtTemp = GetTheFirstDayOfWeek(GetTheFirstDayOfMonth(dt)).Date;

    DateTime dtEnd = GetTheLastDayOfMonth(dt).Date;

 

    for (int i = 0; i < 6; i++)

    {

        if (dtTemp.AddDays(i*7) <= dtEnd)

        {

            dtList.Add(dtTemp.AddDays(i*7));

        }

    }

 

    return dtList.ToArray();

}

 

/// <summary>

/// ���o�Y����b�өu���Ĥ@��

/// </summary>

/// <param name="dt">�Y���</param>

/// <returns>�Y����b�өu���Ĥ@��</returns>

public static DateTime GetTheFirstDayOfQuarter(DateTime dt)

{

    if (dt >= new DateTime(dt.Year, 1, 1) && dt <= new DateTime(dt.Year, 3, DateTime.DaysInMonth(dt.Year, dt.Month), 23, 59, 59))

    {

        return new DateTime(dt.Year, 1, 1);

    }

    else if (dt >= new DateTime(dt.Year, 4, 1) && dt <= new DateTime(dt.Year, 6, DateTime.DaysInMonth(dt.Year, dt.Month), 23, 59, 59))

    {

        return new DateTime(dt.Year, 4, 1);

    }

    else if (dt >= new DateTime(dt.Year, 7, 1) && dt <= new DateTime(dt.Year, 9, DateTime.DaysInMonth(dt.Year, dt.Month), 23, 59, 59))

    {

        return new DateTime(dt.Year, 7, 1);

    }

    else

    {

        return new DateTime(dt.Year, 10, 1);

    }

}

 

/// <summary>

/// ���o�Y����b�өu���̫�@��

/// </summary>

/// <param name="dt">�Y���</param>

/// <returns>�Y����b�өu���̫�@��</returns>

public static DateTime GetTheLastDayOfQuarter(DateTime dt)

{

    if (dt >= new DateTime(dt.Year, 1, 1) && dt <= new DateTime(dt.Year, 3, DateTime.DaysInMonth(dt.Year, dt.Month), 23, 59, 59))

    {

        return new DateTime(dt.Year, 3, DateTime.DaysInMonth(dt.Year, dt.Month));

    }

    else if (dt >= new DateTime(dt.Year, 4, 1) && dt <= new DateTime(dt.Year, 6, DateTime.DaysInMonth(dt.Year, dt.Month), 23, 59, 59))

    {

        return new DateTime(dt.Year, 6, DateTime.DaysInMonth(dt.Year, dt.Month));

    }

    else if (dt >= new DateTime(dt.Year, 7, 1) && dt <= new DateTime(dt.Year, 9, DateTime.DaysInMonth(dt.Year, dt.Month), 23, 59, 59))

    {

        return new DateTime(dt.Year, 9, DateTime.DaysInMonth(dt.Year, dt.Month));

    }

    else

    {

        return new DateTime(dt.Year, 12, DateTime.DaysInMonth(dt.Year, dt.Month));

    }

}

 

/// <summary>
/// ���o�Y����b�өu�C�Ӥ몺�Ĥ@�ѦC��
/// </summary>
/// <param name="dt">�Y���</param>
/// <returns>���o�Y����b�өu�C�Ӥ몺�Ĥ@�ѦC��</returns>
public static DateTime[] GetTheFirstDaysOfMonthInQuarter(DateTime dt)
{
    List<DateTime> dtList = new List<DateTime>();
    DateTime dtTemp = GetTheFirstDayOfQuarter(dt);
    DateTime dtEnd = GetTheLastDayOfQuarter(dt); 

    for (int i = 0; i < 3; i++)
    {
        if (new DateTime(dt.Year, dtTemp.AddMonths(i).Month, 1) <= dtEnd)
        {
            dtList.Add(new DateTime(dt.Year, dtTemp.AddMonths(i).Month, 1));
        }
    } 

    return dtList.ToArray();
} 

/// <summary>
/// ���o�Y����b��~���Ĥ@��
/// </summary>
/// <param name="dt">�Y���</param>
/// <returns>�Y����b��~���Ĥ@��</returns>
public static DateTime GetTheFirstDayOfYear(DateTime dt)
{
    return new DateTime(dt.Year, 1, 1);
} 

/// <summary>
/// ���o�Y����b��~���̫�@��
/// </summary>
/// <param name="dt">�Y���</param>
/// <returns>�Y����b��~���̫�@��</returns>
public static DateTime GetTheLastDayOfYear(DateTime dt)
{
    return new DateTime(dt.Year, 12, DateTime.DaysInMonth(dt.Year, 12));
}

 

/// <summary>
/// ���o�Y������~�C�@�u���Ĥ@�ѦC��
/// </summary>
/// <param name="dt">�Y���</param>
/// <returns>�Y������~�C�@�u���Ĥ@�ѦC��</returns>
public static DateTime[] GetTheFirstDaysOfQuarterInYear(DateTime dt)
{
    List<DateTime> dtList = new List<DateTime>();
    dtList.Add(new DateTime(dt.Year, 1, 1));
    dtList.Add(new DateTime(dt.Year, 4, 1));
    dtList.Add(new DateTime(dt.Year, 7, 1));
    dtList.Add(new DateTime(dt.Year, 10, 1)); 

    return dtList.ToArray();
}