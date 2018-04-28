namespace StarDataTool
{
    class Program
    {
        static void Main()
        {
            var gog = new GaiaOneProcessor();
            gog.Process();

            //var pcg = new PointCloudGenerator();
            //pcg.Generate(@"C:\dev\temp\random10m.dat", 10_000_000);
        }
    }
}

/*

def to_dms(dd):
    abs_dd = abs(dd)
    sign = dd.apply(np.sign).astype(int)
    degs = abs_dd.astype(int)
    minsec = abs_dd - degs
    mins = (minsec * 60).astype(int)
    secs = (minsec - mins/60) * 3600
    degs *= sign
    return degs, mins, secs


def to_xyz(ra, dec, dist):
    dec_d, dec_m, dec_s = to_dms(dec)
    dec_sign = dec.apply(np.sign)
    b = (abs(dec_d) + (dec_m/60.0) + (dec_s/3600.0)) * dec_sign
    x = (dist * b.apply(np.cos)) * ra.apply(np.cos)
    y = (dist * b.apply(np.cos)) * ra.apply(np.sin)
    z = dist * b.apply(np.sin)
    return x, y, z


def to_absolute_mag(apparent_mag, distance):
    return apparent_mag - 5.0 * np.log(abs(distance)/10.0)


def process(df):
    df["dist"] = to_parsecs(df["parallax"])
    df["x"], df["y"], df["z"] = to_xyz(df["ra"], df["dec"], df["dist"])
    df["abs_mag"] = to_absolute_mag(df["phot_g_mean_mag"], abs(df["dist"]))
    return df[["x","y","z","abs_mag"]]



*/
