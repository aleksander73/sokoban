public class Utility {
	public static string SimplifyInstanceName(string name) {
		return name.Replace("(Clone)", "").ToLower();
	}
}
