using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public delegate void SomeDele();

    /* i == 1 : Fade in, i == -1 : Fade out */
    public IEnumerator Fade (int i)
	{
		float alpha = 0;

		if ( i == -1 )
		{
			alpha = 0f;
			gameObject.GetComponent<SpriteRenderer> ().color = new Color ( 0, 0, 0, alpha );

            while ( true )
			{
				alpha = gameObject.GetComponent<SpriteRenderer> ().color.a;

				if ( alpha >= 1 ) break;

				alpha += 0.03f;
				gameObject.GetComponent<SpriteRenderer> ().color = new Color ( 0, 0, 0, alpha );

				yield return null;
			}
		}

		else if ( i == 1 )
		{
			alpha = 1f;
			gameObject.GetComponent<SpriteRenderer> ().color = new Color ( 0, 0, 0, alpha );

			while ( true )
			{
				alpha = gameObject.GetComponent<SpriteRenderer> ().color.a;

				if ( alpha <= 0f ) break;

				alpha -= 0.03f;
				gameObject.GetComponent<SpriteRenderer> ().color = new Color ( 0, 0, 0, alpha );

				yield return null;
			}

			alpha = 0f;
			gameObject.GetComponent<SpriteRenderer> ().color = new Color ( 0, 0, 0, alpha );
		}
	}

    public IEnumerator FadeImage(int i)
    {
        float alpha = 0;

        if (i == -1)
        {
            gameObject.SetActive(true);
            alpha = 0f;
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, alpha);

            while (true)
            {
                alpha = gameObject.GetComponent<Image>().color.a;

                if (alpha >= 1) break;

                alpha += 0.03f;
                gameObject.GetComponent<Image>().color = new Color(0, 0, 0, alpha);

                yield return null;
            }
        }

        else if (i == 1)
        {
            gameObject.SetActive(false);
            alpha = 1f;
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, alpha);

            while (true)
            {
                alpha = gameObject.GetComponent<Image>().color.a;

                if (alpha <= 0f) break;

                alpha -= 0.03f;
                gameObject.GetComponent<Image>().color = new Color(0, 0, 0, alpha);

                yield return null;
            }

            alpha = 0f;
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
        }
    }

    public IEnumerator Fade(SomeDele some_dele, int i)
    {
        float alpha = 0;

        if (i == -1)
        {
            alpha = 0f;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, alpha);

            while (true)
            {
                alpha = gameObject.GetComponent<SpriteRenderer>().color.a;

                if (alpha >= 1) break;

                alpha += 0.03f;
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, alpha);

                yield return null;
            }
            some_dele();
        }

        else if (i == 1)
        {
            alpha = 1f;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, alpha);

            while (true)
            {
                alpha = gameObject.GetComponent<SpriteRenderer>().color.a;

                if (alpha <= 0f) break;

                alpha -= 0.03f;
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, alpha);

                yield return null;
            }

            alpha = 0f;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, alpha);
            some_dele();
        }
    }

    public IEnumerator FadeImage(SomeDele some_dele, int i)
    {
        float alpha = 0;

        if (i == -1)
        {
            gameObject.SetActive(true);
            alpha = 0f;
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, alpha);

            while (true)
            {
                alpha = gameObject.GetComponent<Image>().color.a;

                if (alpha >= 1) break;

                alpha += 0.03f;
                gameObject.GetComponent<Image>().color = new Color(0, 0, 0, alpha);

                yield return null;
            }
            yield return new WaitForSeconds(1f);
            some_dele();
        }

        else if (i == 1)
        {
            gameObject.SetActive(false);
            alpha = 1f;
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, alpha);

            while (true)
            {
                alpha = gameObject.GetComponent<Image>().color.a;

                if (alpha <= 0f) break;

                alpha -= 0.03f;
                gameObject.GetComponent<Image>().color = new Color(0, 0, 0, alpha);

                yield return null;
            }

            alpha = 0f;
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, alpha);

            yield return new WaitForSeconds(1f);
            some_dele();
        }
    }
}
