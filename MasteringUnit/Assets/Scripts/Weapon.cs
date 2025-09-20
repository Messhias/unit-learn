using Contracts;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    #region *** Editor components ***

    [SerializeField, Tooltip("Pause movement after an attack?")]
    private float _pauseMovementMax = 1.0f;
    private float _pauseMovementTimer;
    
    #endregion
    
    private GameObject _attachmentParent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (_pauseMovementTimer > 0f)
        {
            _pauseMovementTimer -=  Time.deltaTime;
            return;
        }

        if (_attachmentParent)
        {
            // reposition the weapon gfx in relation to whoever
            // has this weapon equiped.
            Transform tr = _attachmentParent.transform;
            transform.position = tr.position;
            transform.localEulerAngles  = tr.localEulerAngles;
        }
    }

    public void SetAttachmentParent(GameObject attachment)
    {
        _attachmentParent = attachment;
    }

    public bool IsMovementPaused()
    {
        return _pauseMovementTimer > 0f;
    }

    public void OnAttack(Vector3 facing)
    {
        throw new System.NotImplementedException();
    }
}
