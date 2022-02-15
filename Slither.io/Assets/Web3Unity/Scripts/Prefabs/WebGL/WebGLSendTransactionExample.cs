using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLSendTransactionExample : MonoBehaviour
{
    
    async public void OnSendTransaction()
    {
        // account to send to
        string to = "0x0176Af71cb752df8Cd79595bA70092F9fF539624";
        // amount in wei to send
        string value = "100000000000000";
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        // connects to user's browser wallet (metamask) to send a transaction
        try {
            string response = await Web3GL.SendTransaction(to, value, gasLimit, gasPrice);
            Debug.Log(response);
        } catch (Exception e) {
            Debug.LogException(e, this);
        }
    }
}
