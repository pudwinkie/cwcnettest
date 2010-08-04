
namespace chuiwenchiu.functional
{
	// »¼°j¨ç¦¡»²§U
	// var fac = Fix<int, int>(f => x => x <= 1 ? 1 : x * f(x - 1));
	// var fib = Fix<int, int>(f => x => x <= 1 ? 1 : f(x - 1) + f(x - 2));
	static Func<T, TResult> Fix<T, TResult>(Func<Func<T, TResult>, Func<T, TResult>> f)
	{
		return x => f(Fix(f))(x);
	}

	// var gcd = Fix<int, int, int>(f => (x, y) => y == 0 ? x : f(y, x % y));
	static Func<T1, T2, TResult> Fix<T1, T2, TResult>(Func<Func<T1, T2, TResult>, Func<T1, T2, TResult>> f)
	{
		return (x, y) => f(Fix(f))(x, y);
	}
}