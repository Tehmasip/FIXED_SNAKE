using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderBoardPlayFab : MonoBehaviour {
    public static LeaderBoardPlayFab leaderBoard;
    public Text userEmailText;
    public Text userPasswordText;
    public Text userNameText;
    public Text errorMessgaeText;
    public GameObject loginScreen;
    public GameObject SuccessfulyLoginScreen;
    public GameObject LeaderBoardScreen;
    public GameObject ErrorScreen;
    public GameObject leaderBoardInstance;
    public GameObject DailyLeaderContent;
    public GameObject EnterCompleteInfoScreen;
    public GameObject PlayerDetailScreen;
    public GameObject WeeklyLeaderContent;
    private string userEmail;
    private string userPassword;
    private string userName;
    private int playerLogin;
    private void OnEnable() {
        if (LeaderBoardPlayFab.leaderBoard == null) {
            LeaderBoardPlayFab.leaderBoard = this;
        } else {
            if (LeaderBoardPlayFab.leaderBoard != this) {
                Destroy(this.gameObject);
            }
        }
      //  DontDestroyOnLoad(this.gameObject);
    }
    public void CheckPlayerDetail() {
        if ((!string.IsNullOrEmpty(userNameText.text))&&(!string.IsNullOrEmpty(userEmailText.text))) {
            SetUserNameAndEmail();
            PlayerDetailScreen.SetActive(false);
        } else {
            EnterCompleteInfoScreen.SetActive(true);
        }
    }
    #region Login
    public void Start() {
        //Note: Setting title Id here can be skipped if you have set the value in Editor Extensions already.
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) {
            PlayFabSettings.TitleId = "144"; // Please change this value to your own titleId from PlayFab Game Manager
        }
        /* if (PlayerPrefs.HasKey("EMAIL")) {
             userName = PlayerPrefs.GetString("NAME");
             userEmail = PlayerPrefs.GetString("EMAIL");
             userPassword = PlayerPrefs.GetString("PASSWORD");
             var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
             PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
         }*/
    }

    public InputField inputField;
    public string abc;
    public void inin()
    {
        abc = inputField.text;
        
    }
    
    public void goIn()
    { PlayerPrefs.SetString("Account","uzxrtyaq268");
        var request = new LoginWithCustomIDRequest { CustomId = PlayerPrefs.GetString("Account"), CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }
    private void OnLoginSuccess(LoginResult result) {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.SetString("NAME", userName);
        PlayerPrefs.SetString("EMAIL", userEmail);
        // PlayerPrefs.SetString("PASSWORD", userPassword);
        GetUserData(result.PlayFabId);
        GetSats(); GetSatsWeekly();
        loginScreen.SetActive(false);
        SuccessfulyLoginScreen.SetActive(true);
    }
    void SetUserData(string s) {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
            Data = new Dictionary<string,string>() {
            {"playerLogin",s},
        }
        },
        result => Debug.Log("Successfully updated player login data"),
        error => {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    void GetUserData(string myPlayFabeId) {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
            PlayFabId = myPlayFabeId,
            Keys = null
        }, result => {
            Debug.Log("Got user data:");
            if (result.Data == null || result.Data.ContainsKey("playerLogin")) {
                if (!((result.Data["playerLogin"].Value) == "b")) {
                    PlayerDetailScreen.SetActive(true); SetUserData("b");
                } 
                Debug.Log(" player login"+ result.Data["playerLogin"].Value); } 
            else { Debug.Log("No Values: " ); PlayerDetailScreen.SetActive(true); SetUserData("b");
            }
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    private void SetUserNameAndEmail() {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = userNameText.text }, OnDisplayName, OnLoginFailure);
        PlayFabClientAPI.AddOrUpdateContactEmail(new AddOrUpdateContactEmailRequest { EmailAddress = userEmailText.text }, OnDisplayEmail, OnLoginFailure);
        GetSats();
    }
    private IEnumerator DisplayError(string error) {
        ErrorScreen.SetActive(true);
        errorMessgaeText.text = error;
        yield return new WaitForSeconds(2);
        ErrorScreen.SetActive(false);
    }
    private void OnRegisterSuccess(RegisterPlayFabUserResult result) {
        Debug.Log("Congratulations, you made your first successful API call!");
        //PlayerPrefs.SetString("EMAIL", userEmail);
        //PlayerPrefs.SetString("NAME", userName);
        //PlayerPrefs.SetString("PASSWORD", userPassword);
        //PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = userName }, OnDisplayName, OnLoginFailure);
        GetSats(); GetSatsWeekly();
        loginScreen.SetActive(false);
        //if())
        SuccessfulyLoginScreen.SetActive(true);
    }
    private void OnRegisterfailure(PlayFabError error) {
        StartCoroutine(DisplayError(error.GenerateErrorReport()));
        //   Debug.Log(error.GenerateErrorReport());
    }
    private void OnLoginFailure(PlayFabError error) {
        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = userName };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterfailure);
    }
    private void OnDisplayName(UpdateUserTitleDisplayNameResult result) {
        Debug.Log(result.DisplayName + "is your new display name");
    }
    private void OnDisplayEmail(AddOrUpdateContactEmailResult results) {
        Debug.Log(results.CustomData+"is your email");
    }
    public void GetUserEmail() {
        userEmail = userEmailText.text; //Debug.Log(userEmail);
    }
    public void GetUsername() {
        userName = userNameText.text;
    }
    public void GetUserPasword() {
        userPassword = userPasswordText.text;
    }
    public void OnClickLogin() {
        GetUserEmail(); GetUsername(); GetUserPasword();
        var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }
    #endregion Login
    public void LoadNextScene() {
        SceneManager.LoadScene(2);
    }
    public int playerScore;
    #region PlayerStatistics
    public void SetStats() {
        //Debug.Log("scrggggggggggggggg"+PlayerPrefs.GetInt("BestScore"));
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate> {
             new StatisticUpdate{StatisticName="PlayerScores",Value=PlayerPrefs.GetInt("BestScore")}
            }
        },
        result => { Debug.Log("User Statistics update"); },
        error => { Debug.Log(error.GenerateErrorReport()); });
    }
    private void GetSats() {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStats,
            error => Debug.Log(error.GenerateErrorReport())
            );
    }
    private void OnGetStats(GetPlayerStatisticsResult result) {
        Debug.Log("Recieved following statistics");
        foreach (var eachStat in result.Statistics) {
            Debug.Log("Statictis( " + eachStat.StatisticName + "):" + eachStat.Value);
            playerScore = eachStat.Value;
        }
    }
 
    public void SetStatsWeekly() {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate> {
             new StatisticUpdate{StatisticName="UpdateWeekly"  ,Value=PlayerPrefs.GetInt("BestScore")}
            }
        },
        result => { Debug.Log("User Statistics update"); },
        error => { Debug.Log(error.GenerateErrorReport()); });
    }
    private void GetSatsWeekly() {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatsWeekly,
            error => Debug.Log(error.GenerateErrorReport())
            );
    }
    private void OnGetStatsWeekly(GetPlayerStatisticsResult result) {
        Debug.Log("Recieved following statistics");
        foreach (var eachStat in result.Statistics) {
            //Debug.Log("Statictis( " + eachStat.StatisticName + "):" + eachStat.Value);
            playerScore = eachStat.Value;
        }
    }
    #endregion
    #region LeaderBoard
    public void GetDailyLeaderBoard() {
        var requestLeaderBoard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "PlayerScores", MaxResultsCount = 10 };
        PlayFabClientAPI.GetLeaderboard(requestLeaderBoard, OnGetDailyLeaderBoard, OnErrorLeaderBoard);
    }
    private void OnGetDailyLeaderBoard(GetLeaderboardResult result) {
       // LeaderBoardScreen.SetActive(true);
        for (int i = 0; i < DailyLeaderContent.transform.childCount; i++) {
            Destroy(DailyLeaderContent.transform.GetChild(i).gameObject);
        }
        foreach (PlayerLeaderboardEntry player in result.Leaderboard) {
            GameObject playerProperties = Instantiate(leaderBoardInstance, DailyLeaderContent.transform);
            playerProperties.transform.GetChild(0).GetComponent<Text>().text = player.DisplayName;
            playerProperties.transform.GetChild(1).GetComponent<Text>().text = player.StatValue.ToString();
            Debug.Log(player.DisplayName + ":" + player.StatValue);
        }
    }
    public void GetWeeklyLeaderBoard() {
        var requestLeaderBoard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "UpdateWeekly", MaxResultsCount = 10 };
        PlayFabClientAPI.GetLeaderboard(requestLeaderBoard, OnGetWeeklyLeaderBoard, OnErrorLeaderBoard);
    }
    private void OnGetWeeklyLeaderBoard(GetLeaderboardResult result) {
     //   LeaderBoardScreen.SetActive(true);
        for (int i = 0; i < WeeklyLeaderContent.transform.childCount; i++) {
            Destroy(WeeklyLeaderContent.transform.GetChild(i).gameObject);
        }
        foreach (PlayerLeaderboardEntry player in result.Leaderboard) {
            Debug.Log("weeklyleaderboard");
            GameObject playerProperties = Instantiate(leaderBoardInstance, WeeklyLeaderContent.transform);
            playerProperties.transform.GetChild(0).GetComponent<Text>().text = player.DisplayName;
            playerProperties.transform.GetChild(1).GetComponent<Text>().text = player.StatValue.ToString();
            Debug.Log(player.DisplayName + ":" + player.StatValue);
        }
    }
    public void ShowLeaderBoard() {
        SetStats();
        GetSats();
        SetStatsWeekly(); 
        LeaderBoardScreen.SetActive(true);
        GetWeeklyLeaderBoard();
        GetDailyLeaderBoard();
    }
    private void OnErrorLeaderBoard(PlayFabError error) {
        //Debug.log(error.generateerrorreport());
    }
    #endregion LeaderBoard
}















