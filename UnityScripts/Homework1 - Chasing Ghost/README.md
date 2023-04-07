# Unity Homework Report
## Rule 1: Player controls
```c#
float speed = 6F;
horizontal = Input.GetAxisRaw("Horizontal");
vertical = Input.GetAxisRaw("Vertical");
rb.velocity = new Vector2(horizontal * speed, vertical * speed);
```
I did the player controls as simple as possible. 6F speed is fast enough to have fun.

## Rule 2: Playing are restriction with walls
Walls have static Rigidbody2D components so that Ghosts can bounce off of them and Player cannot faze through them. 

## Rule 3: Ghost mechanics
This is the code that handles most of the Ghost logic

```c#
void generateGhosts() {
	for (int i = 0; i < level; i++) {
		GameObject g = Instantiate(
		ghost,
		overlyComplicatedPos(),
		Quaternion.identity
		);
		if (i == 0) {
			// initialize the crazy ghost (the one that follows)
			g.GetComponent<GhostScript>().makeItCrazy();
		}
		int color = randomColor();
		if (color == 0) {
			g.GetComponent<Renderer>().material.color = Color.magenta;
		} else if (color == 1) {
			g.GetComponent<Renderer>().material.color = Color.green;
		} else {
			g.GetComponent<Renderer>().material.color = Color.cyan;
		}
		ghosts.Add(g);
	}
}
// snip

private Vector2 overlyComplicatedPos() {
	float x = randomPos(XMIN, XMAX); // returns a random value between [XMIN, XMAX)
	float y = randomPos(YMIN, YMAX);
	float playerX = gameObject.transform.position.x;
	float playerY = gameObject.transform.position.y;
	// make sure it's not too close
	while (Abs(playerX - x) < 3f) {
		x = randomPos(XMIN, XMAX);
	}
	while (Abs(playerY - y) < 3f) {
		y = randomPos(YMIN, YMAX);
	}
	return new Vector2(x,y);
}
```

the for loop instantiates level amounts of Ghosts, changes them to a random colour, and adds them to `ghosts` list. The `overlyComplicatedPos` method gets a random float and player's current position. It checks if the random values, `x` and `y`, are too close to player's current position. While this check is true, it gets another random float, until it finds a far enough position. 3f gives a good personal space for the player. 
Ghosts have a bouncy material which makes them bounce off of static walls.

## Rule 4: Prize Collection
Prizes are GameObjects with Sprite Renderer and BoxCollider (with isTrigger checked) components. I have two lists to manage the prizes. 

```c#
public List<GameObject> points = new List<GameObject>();
private List<GameObject> pointsInLevel = new List<GameObject>();
```

`points` list holds the individual prefabs of the prizes. This is used for creating the correct number of prizes for each level as well as checking if the player is taking the prizes in correct order. The list is public and I put the prefabs manually in the list in Unity Editor.
`pointsInLevel` list holds the points in the current level. At the start of each level, points are instantiated from `points` and added to `pointsInLevel` list. 
Holding the references of the points help with destroying and clearing out the prizes when Player gets captured by a Ghost. `points` list does not hold the references of the prizes on the screen and they cannot be destroyed.

```c#
void generatePoints() {
	for (int i = 0; i < level; i++) {
		GameObject point = Instantiate(
			points[i],
			new Vector2(randomPos(XMIN, XMAX), randomPos(YMIN, YMAX)),
			Quaternion.identity
		);
		pointsInLevel.Add(point);
	}
}

// snip

private void OnTriggerEnter2D(Collider2D other) {
	if (other.CompareTag("Point")) {
		int charVal = pointIdx + '0';
		if (other.name.Contains((char) charVal)) {
			pointIdx++;
			Destroy(other.gameObject);
		}
		if (pointIdx == level && level <= 10) {
			ghosts.ForEach(ghost => Destroy(ghost));
			ghosts.Clear();
			level++;
			pointIdx = 0;
			if (level < 10) {
				// continue
				generateGhosts();
				generatePoints();
				return;
			}
			generateEndScreen();
		}
	}
}
```

The logic for handling the collection order is done in `OnTriggerEnter2D` method.
I decided to check the order by checking the name of the points. `pointIdx` starts from zero, and the second if checks for 'numbers_0', the name of the 0 prize. This is enough to collect prizes in correct order.
Then I realized that I could handle most of the game logic in this method. The third if statement does that. It is executed when the final prize is collected. Ghosts in that level are destroyed with `ForEach`, the `ghosts` list is cleared, `level` is incremented, and `pointIdx` is set to zero. The next level is generated if the level is less than 10, else the end screen is shown. 

## Rule 5: Levels
You have seen the variable `level` in previous code blocks. The `level` field in PlayerScript enables me to handle the level logic in this game by integrating it to methods. It's very simple.

```c#
public int level = 1;  // field is initialized

// other fields

void Start() {
	generatePoints();
	generateGhosts();
}
```

The code I showed in Rule 3 and Rule 4 are first called here. Later they are called in `OnTriggerEnter2D` until the end of the game.

## Rule 6: Ghost Chasing The Player
If you recall the ominious line of code in `generateGhosts` method, that's where magic starts. Let's take a look at it again.
```c#
if (i == 0) {
	// initialize the crazy ghost (the one that follows)
	g.GetComponent<GhostScript>().makeItCrazy();
}
```
The *magic* happens in GhostScript, so take a look.

```c#
bool crazy = false;
GameObject player;

// snip

void Update() {
	if (crazy) crazyMove();
}

void crazyMove() {
	// directions to follow
	float x = player.transform.position.x - gameObject.transform.position.x;
	float y = player.transform.position.y - gameObject.transform.position.y;
	float magnitude = Mathf.Sqrt(x*x + y*y);
	float xNormal = 3f * x / magnitude;
	float yNormal = 3f * y / magnitude;
	rb.velocity = new Vector2(xNormal, yNormal);
}

// snip

public void makeItCrazy() {
	crazy = true;
	player = GameObject.Find("Player");
}
```

By default, Ghosts aren't crazy. Only the Ghost with index 0 is crazy, which makes it chase the Player. Crazy Ghost has its own method for chasing, which is invoked in Update. 
First, it gets the distances between Player and Ghost. Then, the magnitude is calculated, which is used for getting the unit vector that points to Player. Also multiplied by a constant for desired difficulty. 
Uncrazy Ghosts get a random value between some interval for their velocity.

## Rule 7: End Screen
There are two ways to trigger the end screen: Finishing the 10 levels (which you can see above), and getting captured by a Ghost. 
At first I was thinking of using Image or some UI element to show the end screen but I didn't want to overcomplicate it. So I just initilialized an object. 

```c#
private void generateEndScreen() {
	GameObject point = Instantiate(
			points[level],
			new Vector2(0, -3F), 
			Quaternion.identity
	);
	endScreen = Instantiate(
				endScreen,
				new Vector3(0,0,0),
				Quaternion.identity
	);
	Time.timeScale = 0;
}

private void OnCollisionEnter2D(Collision2D other) {
	if (other.gameObject.CompareTag("Ghost")){
		Destroy(gameObject);
		ghosts.ForEach(ghost => Destroy(ghost));
		ghosts.Clear();
		pointsInLevel.ForEach(p => Destroy(p));
		pointsInLevel.Clear();
		generateEndScreen();
	}
}
```

`generateEndScreen` creates an object with the "Game Over" sprite and the point of the current level just below it. 

`OnCollisionEnter2D` also triggers the `generateEndScreen`, with some extra steps we haven't covered. Since, this is called when the Player is captured, we have to deal with the remaining points. Again, `ForEach` is used for destroying points. 
