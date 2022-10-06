namespace Examples.TankArena.Scripts.Extensions {
	public static class DoubleExtension {
		
		public static double Factorial(this double i) {
			if (i <= 1)
				return 1;
			return i * Factorial(i - 1);
		}
		
	}
}