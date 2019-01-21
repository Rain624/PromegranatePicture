/****************************************************************************
 *
 * Copyright (c) 2018 Rain
 *
 ****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Linq;

namespace Qdisa
{
    public class MessageHandle
    {
        private byte[] data = new byte[1024];
        private int startIndex = 0;//我们存取了多少个字节的数据在数组里面


        public byte[] Data
        {
            get { return data; }
        }
        public int StartIndex
        {
            get { return startIndex; }
        }
        public int RemainSize
        {
            get { return data.Length - startIndex; }
        }
        /// <summary>
        /// 解析数据或者叫做读取数据
        /// </summary>
        public void ReadMessage(int newDataAmount,Action<string> processDataCallBack)
        {
            startIndex += newDataAmount;
            while (true)
            {
                //粘包分包
                if (startIndex <4) return;
   
                int count = BitConverter.ToInt32(data, 0);//占有4个字节
       
                if ((startIndex - 4) >= count)
                {
                    string s = Encoding.UTF8.GetString(data, 4, count);
                    if (s.Length == 0)
                    {
                        startIndex -= (count + 4);
                        return;
                    }
                    processDataCallBack(s);
                    Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                    startIndex -= (count + 4);
                }
                else
                {

                    break;
                }
            }

        }

        public static byte[] PackData(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int dataAmount = dataBytes.Length;
            byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);
            byte[] newBytes = dataAmountBytes.Concat(dataBytes).ToArray();

            return newBytes;
            //return dataAmountBytes.Concat(dataBytes).ToArray<byte>();      
            
        }


    }
}


