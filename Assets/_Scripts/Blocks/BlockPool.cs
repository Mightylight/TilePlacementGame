using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class BlockPool : MonoBehaviour
{
    //Block data
    [SerializeField] private BlockGroup[] _blockGroups;
    [SerializeField] private Button[] _selectButtons;
    private BlockButton[] _blockButtons;
    private List<BlockData> _storedBlocks = new List<BlockData>();
    
    //Misc data
    private Random rand = new Random();

    private void Awake()
    {
        //Put all playable blocks in one list and shuffle it:
        foreach (BlockGroup group in _blockGroups)
        {
            _storedBlocks.AddRange(Enumerable.Repeat(group.BlockData, group.Amount));
        }
        _storedBlocks = _storedBlocks.OrderBy(x => rand.Next()).ToList();
        
        //Setup select buttons:
        _blockButtons = new BlockButton[_selectButtons.Length];
        for (int i = 0; i < _selectButtons.Length; i++)
        {
            Button button = _selectButtons[i];
            BlockButton blockButton = button.AddComponent<BlockButton>();
            _blockButtons[i] = blockButton;
        }
        GetRandomTiles(true);
    }

    public void GetRandomTiles(bool overrideHandEmpty = false)
    {
        Debug.Log($"HAND EMPTY: {HandEmpty()}");
        if (!HandEmpty() && !overrideHandEmpty)
            return;
        
        foreach (BlockButton button in _blockButtons)
        {
            if (_storedBlocks.Count == 0)
            {
                Debug.LogError("AMOUNT OF STORED BLOCKS NOT DIVIDABLE BY 0!!!");
                return;
            }

            button.Assign(_storedBlocks[0]);
            _storedBlocks.RemoveAt(0);
        }

        if (_storedBlocks.Count == 0)
        {
            Debug.Log("NO MORE TILES, GAME END (OR SOMETHING)");
            Destroy(gameObject);
        }
    }

    private bool HandEmpty()
    {
        foreach (BlockButton button in _blockButtons)
        {
            if (button.gameObject.activeSelf)
                return false;
        }
        return true;
    }

    [Serializable]
    private class BlockGroup
    {
        [SerializeField] private BlockData _blockData;
        [SerializeField] private int _amount;

        public BlockData BlockData => _blockData;
        public int Amount => _amount;
    }

    private class BlockButton : MonoBehaviour
    {
        private BlockData _blockData;
        private GridInitializer _grid;
        private BlockPool _blockPool;
        private Image _image;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
            _image = GetComponent<Image>();
            //Please forgive me for this code, this is for the proof of concept. Hopefully we can change this whole class to dragging tiles onto the map, instead of using buttons  
            _grid = FindObjectOfType<GridInitializer>();
            _blockPool = FindObjectOfType<BlockPool>();
        }

        public void Assign(BlockData pBlockData)
        {
            _blockData = pBlockData;
            gameObject.SetActive(true);
            _image.color = pBlockData.blockName == "River" ? Color.cyan : Color.gray;
        }

        private void OnClick()
        {
            if (_grid.CurrentBlock is not null)
                return;
            _grid.SetCurrentBlock(_blockData);
            gameObject.SetActive(false);
        }
    }
}
