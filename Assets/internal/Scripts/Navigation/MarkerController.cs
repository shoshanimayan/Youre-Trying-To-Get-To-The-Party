using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerController : MonoBehaviour
{
    [SerializeField] float _animationDuration;
    [SerializeField] float _animationOffset=.3f;

    private Coroutine _animateCo=null;

    public void SetMarker(Vector3 pos)
    {
        if (_animateCo != null)
        {
            return;
            StopCoroutine(_animateCo);
            _animateCo = null;
        }

        transform.position = pos;
        _animateCo= StartCoroutine(AnimateMarker());
    }

    private IEnumerator AnimateMarker()
    {
        Vector3 endPos = new Vector3( transform.position.x, transform.position.y, transform.position.z-.05f);
        Vector3 startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + _animationOffset);
        transform.position = startPos;
        float elapsedTime = 0;

        while (elapsedTime < _animationDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / _animationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _animateCo = null;
    }
}
