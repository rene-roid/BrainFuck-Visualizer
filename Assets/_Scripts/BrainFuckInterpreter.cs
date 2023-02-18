using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace rene_roid {    
    public class BrainFuckInterpreter : MonoBehaviour
    {
        [TextArea(15,20)]
        public string code = "";
        public int codePointer = 0;
        public int dataPointer = 0;
        public int[] data = new int[30000];

        public TMP_InputField inputField;
        public TMP_InputField insertCodeField;
        public Button playCode;
        public TMP_Text outputField;
        public Toggle autoRunCode;
        public TMP_InputField autoRunCodeDelay;
        public Button nextLine;

        
        public GameObject[] cells;

        public GameObject cellPrefab;
        public GameObject cellParent;

        private int moveVis = 70;
        private WaitForSeconds wait = new WaitForSeconds(0.1f);
        private bool nextLinePressed = false;


        private void Start()
        {
            playCode.onClick.AddListener(PlayCode);
            nextLine.onClick.AddListener(NextLine);

            autoRunCodeDelay.text = "0.1";

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = 0;
            }

            // Instantiate the cells for the data and set their position and add them to the cells array
            for (int i = 0; i < 100; i++) // data.Length
            {
                cells[i] = Instantiate(cellPrefab, cellParent.transform);
                cells[i].transform.position = new Vector3(i * moveVis, 0, 0);
            }

            UpdateData();
        }

        public void PlayCode()
        {
            code = insertCodeField.text;
            codePointer = 0;
            dataPointer = 0;
            outputField.text = "Output: ";
            inputField.GetComponent<Image>().color = Color.white;

            wait = new WaitForSeconds(float.Parse(autoRunCodeDelay.text));

            UpdateData();

            StartCoroutine(InterpretCode());
        }

        public void NextLine()
        {
            nextLinePressed = true;
        }

        private IEnumerator InterpretCode()
        {
            while (codePointer < code.Length)
            {
                switch (code[codePointer])
                {
                    case '>':
                        dataPointer++;
                        dataPointer = Mathf.Clamp(dataPointer, 0, data.Length);
                        break;
                    case '<':
                        dataPointer--;
                        dataPointer = Mathf.Clamp(dataPointer, 0, data.Length);
                        break;
                    case '+':
                        data[dataPointer]++;
                        break;
                    case '-':
                        data[dataPointer]--;
                        break;
                    case '.':
                        Debug.Log((char)data[dataPointer]);
                        outputField.text += (char)data[dataPointer];
                        break;
                    case ',':
                        inputField.GetComponent<Image>().color = Color.white;
                        yield return new WaitUntil(() => inputField.text.Length > 0);
                        data[dataPointer] = (int)inputField.text[0];
                        inputField.text = "";
                        inputField.GetComponent<Image>().color = Color.gray;
                        break;
                    case '[':
                        if (data[dataPointer] == 0)
                        {
                            int loopCounter = 1;
                            while (loopCounter > 0)
                            {
                                codePointer++;
                                if (code[codePointer] == '[')
                                {
                                    loopCounter++;
                                }
                                else if (code[codePointer] == ']')
                                {
                                    loopCounter--;
                                }
                            }
                        }
                        break;
                    case ']':
                        if (data[dataPointer] != 0)
                        {
                            int loopCounter = 1;
                            while (loopCounter > 0)
                            {
                                codePointer--;
                                if (code[codePointer] == ']')
                                {
                                    loopCounter++;
                                }
                                else if (code[codePointer] == '[')
                                {
                                    loopCounter--;
                                }
                            }
                            codePointer--;
                        }
                        break;
                }

                UpdateData();
                codePointer++;

                if (autoRunCode.isOn)
                {
                    yield return wait;
                } else
                {
                    yield return new WaitUntil(() => nextLinePressed);
                    nextLinePressed = false;
                }
            }
        }

        private void UpdateData()
        {
            BoldCode();
            for (int i = 0; i < dataPointer + 100; i++)
            {
                if (cells[i] == null)
                {
                    cells[i] = Instantiate(cellPrefab, cellParent.transform);
                }
                cells[i].GetComponentInChildren<TextMeshProUGUI>().text = data[i].ToString();

                // Change the color of the cell if is the data pointer
                if (i == dataPointer)
                {
                    cells[i].GetComponent<Image>().color = Color.red;
                }
                else
                {
                    cells[i].GetComponent<Image>().color = Color.white;
                }
            }

            wait = new WaitForSeconds(float.Parse(autoRunCodeDelay.text) > 0.05f ? float.Parse(autoRunCodeDelay.text) : 0.05f);
            autoRunCodeDelay.text = float.Parse(autoRunCodeDelay.text) > 0.05f ? autoRunCodeDelay.text : "0.05";
        }

        private void BoldCode()
        {
            string newCode = "";
            for (int i = 0; i < code.Length; i++)
            {
                if (i == codePointer)
                {
                    newCode += "<b>" + code[i] + "</b>";
                }
                else
                {
                    newCode += code[i];
                }
            }
            insertCodeField.text = newCode;
        }
    }
}
