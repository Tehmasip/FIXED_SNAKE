using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomCallExample : MonoBehaviour
{
    public Text Num;
    async void Start()
    {
        /*
        // SPDX-License-Identifier: MIT
        pragma solidity ^0.8.0;

        contract AddTotal {
            uint256 public myTotal = 0;

            function addTotal(uint8 _myArg) public {
                myTotal = myTotal + _myArg;
            }
        }
        */
        // set chain: ethereum, moonbeam, polygon etc
        string chain = "ethereum";
        // set network mainnet, testnet
        string network = "rinkeby";
        // smart contract method to call
        string method = "ABC";
        // abi in json format
        //string abi = "[ { \"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"_myArg\", \"type\": \"uint8\" } ], \"name\": \"addTotal\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"myTotal\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
        string abim = "[ 	{ 		\"inputs\": [], 		\"name\": \"ABC\", 		\"outputs\": [ 			{ 				\"internalType\": \"uint256\", 				\"name\": \"\", 				\"type\": \"uint256\" 			} 		], 		\"stateMutability\": \"view\", 		\"type\": \"function\" 	}, 	{ 		\"inputs\": [ 			{ 				\"internalType\": \"uint8\", 				\"name\": \"m\", 				\"type\": \"uint8\" 			} 		], 		\"name\": \"addy\", 		\"outputs\": [], 		\"stateMutability\": \"nonpayable\", 		\"type\": \"function\" 	} ]";
        // address of contract
        string contract = "0xB58e8Cf95704Ce327Dd08BD0a7c4B7B0E8e5E166";
        // array of arguments for contract
        string args = "[]";
        // connects to user's browser wallet to call a transaction
        string response = await EVM.Call(chain, network, contract, abim, method, args);
        // display response in game
        print(response);
        Num.text = response;
    }

    public void CallABC()
    {
        Start();
    }
}
