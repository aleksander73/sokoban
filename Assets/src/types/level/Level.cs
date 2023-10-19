public class Level {
	private readonly Grid<Cell> grid;

	public Level(Grid<Cell> grid) {
		this.grid = grid;
	}

	public Grid<Cell> GetGrid() {
		return this.grid;
	}
}
