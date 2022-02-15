using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebGLSendContractExample : MonoBehaviour
{
    async public void OnSendContract()
    {
        // smart contract method to call
        string method = "addy";
        // abi in json format
        //string abi = "[ { \"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"_myArg\", \"type\": \"uint8\" } ], \"name\": \"addTotal\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"myTotal\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
        string abim = "[ 	{ 		\"inputs\": [], 		\"name\": \"ABC\", 		\"outputs\": [ 			{ 				\"internalType\": \"uint256\", 				\"name\": \"\", 				\"type\": \"uint256\" 			} 		], 		\"stateMutability\": \"view\", 		\"type\": \"function\" 	}, 	{ 		\"inputs\": [ 			{ 				\"internalType\": \"uint8\", 				\"name\": \"m\", 				\"type\": \"uint8\" 			} 		], 		\"name\": \"addy\", 		\"outputs\": [], 		\"stateMutability\": \"nonpayable\", 		\"type\": \"function\" 	} ]";


        // address of contract
        string contract = "0xB58e8Cf95704Ce327Dd08BD0a7c4B7B0E8e5E166";
        // array of arguments for contract
        string args = "[\"5\"]";
        // value in wei
        string value = "0";
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        // connects to user's browser wallet (metamask) to update contract state
        try {
            string response = await Web3GL.SendContract(method, abim, contract, args, value, gasLimit, gasPrice);
            
            Debug.Log(response);
        } catch (Exception e) {
            Debug.LogException(e, this);
        }
    }
}