using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class touch_ : MonoBehaviour {
	
	
	///****Tap_runner game property of rock group (c)2015*****///
	/// Programmer : Dario Ortega _DB009
	/// expanded off of mobile_touch_ asset
	///Thanks to everyone in the Irc chat room at #uiny3d irc.Freenode.net and #ludumdare irc.Afternet.org
	/// //Tijn Rolling average formula and example :D (#ludumdare)
	 
	//******************************* 
	//*******************************
	//***** 	Base Variables	*****
	//******************************* 
	//******************************
	
	
	public float minSwipeDistY;
	
	public float minSwipeDistX;

	private float le_frames = 0;
	
	
	private List<taps> le_taps = new List<taps>(){new taps(),new taps(),new taps(),new taps(),new taps(),new taps(),new taps(),new taps(),new taps(),new taps()};
	
	private List<taps> recent_taps = new List<taps>();
	private List<taps> newtaps = new List<taps>();
	
	
	
	
	CharacterController cc ;
	public enum Player_act{left,right,other};
	public bool isGrounded;
	public Player_act player_previous;


	public float track_pos = 1.0f;

	public float rights=0;
	public float lefts=0;
	
	
	public float mvspd = 0.0f;
	public float sidspd = 0.0f;
	public float player_speed = 20.0F;
	public float jump_height = 5.0f;
	public float max_spd = 30.0F;
	
	//Players default vertical velocity
	public float vervel=0;
	
	public float tap_count = 0;
	public float misses = 0;
	public float points=0;
	private string cur_action ="";

    public GameObject left, right, current,spd_val;
	
	//******************************* 
	//******************************
	//******************************* 
	//******************************
	
	
	void Start(){
		player_previous = Player_act.other;
		
		cc = GetComponent<CharacterController> ();
		
	}
	
	
	void Update() {
		
		
		
		newtaps.Clear ();
		float screen_half = Screen.width / 2;
		
	
		//**Tap_runner scripts **//


		
		
		Text current_text = current.GetComponent<Text> ();
		
		////////////////
		
		
		
		
		if (cur_action != "dmg") {
						////START TOUCH INPUTS READING////////
						int nbTouches = Input.touchCount;
		
						if (nbTouches > 0) {
								cur_action = "";
								for (int i = 0; i < nbTouches; i++) {
										Touch touch = Input.GetTouch (i);
				
										//Debug.Log (touch.fingerId);
				
										TouchPhase phase = touch.phase;
										//print (phase);
				
				
										if (phase == TouchPhase.Began) {
												//Normal script for touch_ asset
					
												//print("New touch detected at position " + touch.position + " , index " + touch.fingerId);
					
												//recent_taps.Add(new taps(Time.realtimeSinceStartup, touch.position, touch.position , touch.fingerId));
												//le_taps(touch.fingerId)=new taps(Time.realtimeSinceStartup, touch.position, touch.position , touch.fingerId));





												///****Tap_runner script*****///
					
												//*******************************
												//**** GET LEFT OR RIGHT TAP ****
												//******************************* 
												//******************************
					
					
												//Debug.Log(touch.fingerId + "finger " );
					
												////LEFT TAP
												/// 
												if (touch.position.x < screen_half) {	///if the position of the current touch that just begain is left than half the screen it was a left touch
														if (player_previous == Player_act.right || player_previous == Player_act.other) {
																//Debug.Log ("left"); 
																if (isGrounded == true) {
																		lefts += 1;
																		tap_count += 1;
																		Text left_text = left.GetComponent<Text> ();
																		left_text.text = lefts.ToString ();
																		player_previous = Player_act.left;
																		recent_taps.Add (new taps (Time.realtimeSinceStartup, touch.position, touch.position, touch.fingerId));
								
								
																}
														}
														le_taps [touch.fingerId] = new taps (Time.realtimeSinceStartup, touch.position, touch.position, touch.fingerId);
						
												} else {////RIGHT TAP
														if (player_previous == Player_act.left || player_previous == Player_act.other) {
							
																//Debug.Log ("right"); 
																if (isGrounded == true) {
																		rights += 1;
																		tap_count += 1;
																		Text right_text = right.GetComponent<Text> ();
																		right_text.text = rights.ToString ();
																		player_previous = Player_act.right;
																		recent_taps.Add (new taps (Time.realtimeSinceStartup, touch.position, touch.position, touch.fingerId));
																}
							
														}
														le_taps [touch.fingerId] = new taps (Time.realtimeSinceStartup, touch.position, touch.position, touch.fingerId);
						
												}
					
					
					
					
												//******************************* 
												//******************************
												//******************************* 
												//******************************
					
										}
				
										if (phase == TouchPhase.Moved) {
					
												//print("Touch index " + touch.fingerId + " moving " + touch.position);
					
												le_taps [touch.fingerId].position = touch.position;
					
					
					
										}
				
				
										if (phase == TouchPhase.Ended) {
					
												//print("Ended Touch index " + touch.fingerId + " ended at position " + touch.position);
					
					
												float swipeDistVertical = (new Vector3 (0, touch.position.y, 0) - new Vector3 (0, le_taps [touch.fingerId].start_pos.y, 0)).magnitude;
												float swipeDistHorizontal = (new Vector3 (touch.position.x, 0, 0) - new Vector3 (le_taps [touch.fingerId].start_pos.x, 0, 0)).magnitude;
					
												float swipeUp = 0.0f;
												float swipeSide = 0.0f;
					
												if (swipeDistVertical < 0) {
														swipeUp = -swipeDistVertical;
												} else {
														swipeUp = swipeDistVertical;
												}
					
					
												if (swipeDistHorizontal < 0) {
														swipeSide = -swipeDistHorizontal;
												} else {
														swipeSide = swipeDistHorizontal;
												}
					
												//Debug.Log(swipeUp + " : " + swipeSide + " Swipes1");
					
					
												if (swipeUp >= swipeSide) {
						
														if (swipeDistVertical >= (Screen.height * .12)) {
							
							
																float swipeValue = Mathf.Sign (touch.position.y - le_taps [touch.fingerId].start_pos.y);
																Debug.Log (swipeValue + "swipe val " + cc.isGrounded);
							
							
							
																if (swipeValue > 0 && isGrounded == true) {//up swipe
																		Debug.Log ("UP");
								
								
																		////Increase vertical velocity to make player jump
																		vervel += jump_height;
								
																		cur_action = "jumping";
								
																} else if (swipeValue < 0 && isGrounded == true) {//down swipe
								
								
								
																		cur_action = "sliding";
																} else {
																		Debug.Log ("ehhh");
																}
							
														} else {
																Debug.Log ("Up Swipe didnt reach lmit");
														}
						
														current_text.text = cur_action;
												}
										else {
						
															  
													if (isGrounded == true && swipeDistHorizontal > (Screen.width * .12)) {
																Debug.Log ("Side Swipe");

								
								float swipeValue = Mathf.Sign (touch.position.x - le_taps [touch.fingerId].start_pos.x);
								//Debug.Log (swipeValue + "swipe val " + cc.isGrounded);

								if(swipeValue > 0 && track_pos != 2){
									track_pos+=1;
																sidspd += 110;
								}
								else if(swipeValue < 0 && track_pos != 0){
									track_pos-=1;
									sidspd -= 110;
								}


														} else {
																Debug.Log ("Side Swipe didnt reach lmit");
																sidspd = 0.0f;
														}
												}
				
					
										}
								}
			
						}
		
		
		
						foreach (taps e in recent_taps) {
								if (e.time >= Time.realtimeSinceStartup - 3) {
										//Debug.Log("recent click");
										newtaps.Add (e);
										//	mvspd+=player_speed;
								}
						}
		
						//Rolling Average for taps can add position check to cover area basic car acceleration
		
						recent_taps.Clear ();
		
						foreach (taps e in newtaps) {
			
								//Debug.Log("recent click");
								recent_taps.Add (new taps (e.time, e.position, e.start_pos, e.id));
			
						}
		
		
						//Debug.Log (taps.Count);
						mvspd = recent_taps.Count;
		
				} else {
			foreach (taps e in recent_taps) {
				if (e.time >= Time.realtimeSinceStartup - 3) {
					//Debug.Log("recent click");
					newtaps.Add (e);
					//	mvspd+=player_speed;
				}
			}
			
			//Rolling Average for taps can add position check to cover area basic car acceleration
			
			recent_taps.Clear ();
			
			foreach (taps e in newtaps) {
				
				//Debug.Log("recent click");
				recent_taps.Add (new taps (e.time, e.position, e.start_pos, e.id));
				
			}
			
			
			//Debug.Log (taps.Count);
			mvspd = -30;
			le_frames+=1;
			if(le_frames>=10)
			{
			cur_action="";
			}
				}
		
		
	}
	
	void FixedUpdate()
	{
		//Debug.Log (sidspd);
		
		Text spd_text = spd_val.GetComponent<Text> ();
		

		
		
		
		if (mvspd > 0) {
			if (isGrounded) {
				mvspd -= 1*Time.deltaTime;
			}
			else{
				mvspd -= 0.6f*Time.deltaTime;
			}
		} else {
			mvspd = 0;
		}
		
		
		
		if (mvspd > max_spd) {
			mvspd = max_spd;
		}
		
		
		if (vervel >0 && mvspd <= 2 ) {
			Debug.Log ("HIIIIII JUMP WHEN NOT MOVING");
			mvspd+=player_speed*2;
		}
		
		spd_text.text = mvspd.ToString ();
		
		Vector3 spd = new Vector3 (sidspd, mvspd, 0);


        //the Movemen function * frames per second
        //cc.Move (spd * Time.deltaTime);
        this.gameObject.transform.Translate(spd*Time.deltaTime, 0);

        sidspd = 0;
	}
	
	void OnControllerColliderHit(ControllerColliderHit col) {

		if (col.gameObject.tag == "Point") {
			Debug.Log ("CharacterController Collision");
			cur_action = "dmg";
			misses+=1;
			Destroy (col.gameObject);
		}
	}
	
}