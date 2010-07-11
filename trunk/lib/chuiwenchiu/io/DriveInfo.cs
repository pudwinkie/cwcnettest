namespcae chuiwenchiu.io{
///
///<example>
/// DriveInfoSystem info = DriveInfo.GetInfo("c:");
///</example>
public sealed class DriveInfo
{
    [DllImport("kernel32.dll", EntryPoint = "GetDiskFreeSpaceExA")]
    private static extern long GetDiskFreeSpaceEx(string lpDirectoryName,
        out long lpFreeBytesAvailableToCaller,
        out long lpTotalNumberOfBytes,
        out long lpTotalNumberOfFreeBytes);

    public static long GetInfo(string drive, out long available, out long total, out long free)
    {
        return GetDiskFreeSpaceEx(drive, out available, out total, out free);
    }

    public static DriveInfoSystem GetInfo(string drive)
    {
        long result, available, total, free;
        result = GetDiskFreeSpaceEx(drive, out available, out total, out free);
        return new DriveInfoSystem(drive, result, available, total, free);
    }
}

public struct DriveInfoSystem
{
    public readonly string Drive;
    public readonly long Result;
    public readonly long Available;
    public readonly long Total;
    public readonly long Free;

    public DriveInfoSystem(string drive, long result, long available, long total, long free)
    {
        this.Drive = drive;
        this.Result = result;
        this.Available = available;
        this.Total = total;
        this.Free = free;
    }
}
}