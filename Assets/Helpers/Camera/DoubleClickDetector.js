#pragma strict

public class DoubleClickDetector {
	private var numberOfClicks : int = 0;
	private var timer : float = 0.0f;
	
	function IsDoubleClick() {
		var isDoubleClick = numberOfClicks == 2;
		if (isDoubleClick)
			numberOfClicks = 0;
		return isDoubleClick;
	}

	function Update () {
		timer += Time.deltaTime;
		
		if (timer > 0.3f) {
			numberOfClicks = 0;
		}
		
		if (Input.GetMouseButtonDown(0)) {
			numberOfClicks++;
			timer = 0.0f;
		}
	}
}