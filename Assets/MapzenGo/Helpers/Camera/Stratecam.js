#pragma strict

import UnityEngine.UI;

// -------------------------- Configuration --------------------------
public var terrain : Terrain;

public var panSpeed : float = 15.0f;
public var zoomSpeed : float = 100.0f;
public var rotationSpeed : float = 50.0f;

public var mousePanMultiplier : float = 0.1f;
public var mouseRotationMultiplier : float = 0.2f;
public var mouseZoomMultiplier : float = 5.0f;

public var minZoomDistance : float = 20.0f;
public var maxZoomDistance : float = 200.0f;
public var smoothingFactor : float = 0.1f;
public var goToSpeed : float = 0.1f;

public var useKeyboardInput : boolean = true;
public var useMouseInput : boolean = true;
public var adaptToTerrainHeight : boolean = true;
public var increaseSpeedWhenZoomedOut : boolean = true;
public var correctZoomingOutRatio : boolean = true;
public var smoothing : boolean = true;
public var allowDoubleClickMovement : boolean = false;

public var allowScreenEdgeMovement : boolean = true;
public var screenEdgeSize : int = 10;
public var screenEdgeSpeed : float = 1.0f;

public var objectToFollow : GameObject;
public var cameraTarget : Vector3;

// -------------------------- Private Attributes --------------------------
private var currentCameraDistance : float;
private var lastMousePos : Vector3;
private var lastPanSpeed : Vector3 = Vector3.zero;
private var goingToCameraTarget : Vector3 = Vector3.zero;
private var doingAutoMovement : boolean = false;
private var doubleClickDetector : DoubleClickDetector;

// -------------------------- Public Methods --------------------------
function Start () {
	currentCameraDistance = minZoomDistance + ((maxZoomDistance - minZoomDistance) / 2.0f);
	lastMousePos = Vector3.zero;
	doubleClickDetector = new DoubleClickDetector();
}

function Update () {
	if (allowDoubleClickMovement) {
		doubleClickDetector.Update();
		UpdateDoubleClick();
	}
	UpdatePanning();
	UpdateRotation();
	UpdateZooming();
	UpdatePosition();
	UpdateAutoMovement();
	lastMousePos = Input.mousePosition;
}

function GoTo(position : Vector3) {
	doingAutoMovement = true;
	goingToCameraTarget = position;
	objectToFollow = null;
}

function Follow(gameObjectToFollow : GameObject) {
	objectToFollow = gameObjectToFollow;
}

// -------------------------- Private Methods --------------------------
private function UpdateDoubleClick() {
	if (doubleClickDetector.IsDoubleClick() && terrain && terrain.GetComponent(Collider)) {
		var cameraTargetY = cameraTarget.y;

		var collider : Collider = terrain.GetComponent(Collider);
		var ray : Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var hit : RaycastHit;
		var pos : Vector3;

		if (collider.Raycast(ray, hit, Mathf.Infinity)) {
			pos = hit.point;
			pos.y = cameraTargetY;
			GoTo(pos);
		}
	}
}

private function UpdatePanning() {
	var moveVector : Vector3 = new Vector3(0, 0, 0);
	if (useKeyboardInput) {
		if (Input.GetKey(KeyCode.A)) {
			moveVector += Vector3(-1, 0, 0);
		}
		if (Input.GetKey(KeyCode.S)) {
			moveVector += Vector3(0, 0, -1);
		}
		if (Input.GetKey(KeyCode.D)) {
			moveVector += Vector3(1, 0, 0);
		}
		if (Input.GetKey(KeyCode.W)) {
			moveVector += Vector3(0, 0, 1);
		}
	}

    if (allowScreenEdgeMovement) {
        if (Input.mousePosition.x < screenEdgeSize) {
            moveVector.x -= screenEdgeSpeed;
        } else if (Input.mousePosition.x > Screen.width - screenEdgeSize) {
            moveVector.x += screenEdgeSpeed;
        }

        if (Input.mousePosition.y < screenEdgeSize) {
            moveVector.z -= screenEdgeSpeed;
        } else if (Input.mousePosition.y > Screen.height - screenEdgeSize) {
            moveVector.z += screenEdgeSpeed;
        }
    }

	if (useMouseInput) {
		if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftShift)) {
			var deltaMousePos : Vector3 = (Input.mousePosition - lastMousePos);
			moveVector += Vector3(-deltaMousePos.x, 0, -deltaMousePos.y) * mousePanMultiplier;
		}
	}

	if (moveVector != Vector3.zero) {
		objectToFollow = null;
		doingAutoMovement = false;
	}

	var effectivePanSpeed = moveVector;
	if (smoothing) {
		effectivePanSpeed = Vector3.Lerp(lastPanSpeed, moveVector, smoothingFactor);
		lastPanSpeed = effectivePanSpeed;
	}

	var oldRotation : float = transform.localEulerAngles.x;
	transform.localEulerAngles.x = 0.0f;
	var panMultiplier : float = increaseSpeedWhenZoomedOut ? (Mathf.Sqrt(currentCameraDistance)) : 1.0f;
	cameraTarget = cameraTarget + transform.TransformDirection(effectivePanSpeed) * panSpeed * panMultiplier * Time.deltaTime;
	transform.localEulerAngles.x = oldRotation;
}

private function UpdateRotation() {
	var deltaAngleH : float = 0.0f;
	var deltaAngleV : float = 0.0f;

	if (useKeyboardInput) {
		if (Input.GetKey(KeyCode.Q)) {
			deltaAngleH = 1.0f;
		}
		if (Input.GetKey(KeyCode.E)) {
			deltaAngleH = -1.0f;
		}
	}

	if (useMouseInput) {
		if (Input.GetMouseButton(2) && !Input.GetKey(KeyCode.LeftShift)) {
			var deltaMousePos : Vector3 = (Input.mousePosition - lastMousePos);
			deltaAngleH += deltaMousePos.x * mouseRotationMultiplier;
			deltaAngleV -= deltaMousePos.y * mouseRotationMultiplier;
		}
	}

	transform.localEulerAngles.y = transform.localEulerAngles.y + deltaAngleH * Time.deltaTime * rotationSpeed;
	transform.localEulerAngles.x = Mathf.Min(80.0f, Mathf.Max(5.0f, transform.localEulerAngles.x + deltaAngleV * Time.deltaTime * rotationSpeed));
}

private function UpdateZooming() {
	var deltaZoom : float = 0.0f;
	if (useKeyboardInput) {
		if (Input.GetKey(KeyCode.F)) {
			deltaZoom = 1.0f;
		}
		if (Input.GetKey(KeyCode.R)) {
			deltaZoom = -1.0f;
		}
	}
	if (useMouseInput) {
		var scroll : float = Input.GetAxis("Mouse ScrollWheel");
		deltaZoom -= scroll * mouseZoomMultiplier;
	}
	var zoomedOutRatio : float = correctZoomingOutRatio ? (currentCameraDistance - minZoomDistance) / (maxZoomDistance - minZoomDistance) : 0.0f;
	currentCameraDistance = Mathf.Max(minZoomDistance, Mathf.Min(maxZoomDistance, currentCameraDistance + deltaZoom * Time.deltaTime * zoomSpeed * (zoomedOutRatio * 2.0f + 1.0f)));
}

private function UpdatePosition() {
	if (objectToFollow != null) {
		cameraTarget = Vector3.Lerp(cameraTarget, objectToFollow.transform.position, goToSpeed);
	}

	transform.position = cameraTarget;
	transform.Translate(Vector3.back * currentCameraDistance);

	if (adaptToTerrainHeight && terrain != null) {
		transform.position.y = Mathf.Max(terrain.SampleHeight(transform.position) + terrain.transform.position.y + 10.0f, transform.position.y);
	}
}

private function UpdateAutoMovement() {
	if (doingAutoMovement) {
		cameraTarget = Vector3.Lerp(cameraTarget, goingToCameraTarget, goToSpeed);
		if (Vector3.Distance(goingToCameraTarget, cameraTarget) < 1.0f) {
			doingAutoMovement = false;
		}
	}
}