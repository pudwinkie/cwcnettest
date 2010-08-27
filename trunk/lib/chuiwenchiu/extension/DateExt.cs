
// 計算日期位於一年中第幾個星期
public static int GetWeekNumber(DateTime date)
{
    var ci = CultureInfo.CurrentCulture;
    return ci.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
}

/// <summary>

/// 取得某日期的 24 小時時刻列表

/// </summary>

/// <param name="dt">某日期</param>

/// <returns>某日期的 24 小時時刻列表</returns>

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

/// 取得某日期在該星期的第一天 (星期日)

/// </summary>

/// <param name="dt">某日期</param>

/// <returns>某日期在該星期的第一天 (星期日)</returns>

public static DateTime GetTheFirstDayOfWeek(DateTime dt)

{

    return dt.AddDays((int) dt.DayOfWeek*-1).Date;

}

 

/// <summary>

/// 取得某日期在該星期的最後一天 (星期六)

/// </summary>

/// <param name="dt">某日期</param>

/// <returns>某日期在該星期的最後一天 (星期六)</returns>

public static DateTime GetTheLastDayOfWeek(DateTime dt)

{

    return dt.AddDays(7 + (int) dt.DayOfWeek*-1 - 1).Date;

}

 

/// <summary>

/// 取得某日期在該月份的第一天 (1 號)

/// </summary>

/// <param name="dt">某日期</param>

/// <returns>某日期在該月份的第一天</returns>

public static DateTime GetTheFirstDayOfMonth(DateTime dt)

{

    return new DateTime(dt.Year, dt.Month, 1);

}

 

/// <summary>

/// 取得某日期在該月份的最後一天

/// </summary>

/// <param name="dt">某日期</param>

/// <returns>某日期在該月份的最後一天</returns>

public static DateTime GetTheLastDayOfMonth(DateTime dt)

{

    return new DateTime(dt.Year, dt.Month + 1, 1).AddDays(-1);

}

 

/// <summary>

/// 取得某日期在該月份每週的第一天列表

/// </summary>

/// <param name="dt">某日期</param>

/// <returns>某日期在該月份每週的第一天列表</returns>

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

/// 取得某日期在該季的第一天

/// </summary>

/// <param name="dt">某日期</param>

/// <returns>某日期在該季的第一天</returns>

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

/// 取得某日期在該季的最後一天

/// </summary>

/// <param name="dt">某日期</param>

/// <returns>某日期在該季的最後一天</returns>

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
/// 取得某日期在該季每個月的第一天列表
/// </summary>
/// <param name="dt">某日期</param>
/// <returns>取得某日期在該季每個月的第一天列表</returns>
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
/// 取得某日期在當年的第一天
/// </summary>
/// <param name="dt">某日期</param>
/// <returns>某日期在當年的第一天</returns>
public static DateTime GetTheFirstDayOfYear(DateTime dt)
{
    return new DateTime(dt.Year, 1, 1);
} 

/// <summary>
/// 取得某日期在當年的最後一天
/// </summary>
/// <param name="dt">某日期</param>
/// <returns>某日期在當年的最後一天</returns>
public static DateTime GetTheLastDayOfYear(DateTime dt)
{
    return new DateTime(dt.Year, 12, DateTime.DaysInMonth(dt.Year, 12));
}

 

/// <summary>
/// 取得某日期於當年每一季的第一天列表
/// </summary>
/// <param name="dt">某日期</param>
/// <returns>某日期於當年每一季的第一天列表</returns>
public static DateTime[] GetTheFirstDaysOfQuarterInYear(DateTime dt)
{
    List<DateTime> dtList = new List<DateTime>();
    dtList.Add(new DateTime(dt.Year, 1, 1));
    dtList.Add(new DateTime(dt.Year, 4, 1));
    dtList.Add(new DateTime(dt.Year, 7, 1));
    dtList.Add(new DateTime(dt.Year, 10, 1)); 

    return dtList.ToArray();
}