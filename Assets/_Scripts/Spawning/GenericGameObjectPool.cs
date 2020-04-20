using System.Collections.Generic;
using UnityEngine;

public class GenericGameObjectPool : MonoBehaviour
{
    [ Tooltip ( @"
  When unpooling an item, we'll grab a random item based on the prefab list, so that the numbers of unpooled items roughly matche the numbers of their prefabs. 
  Since it is likely that there will different variants of the type requested, we'll use the gameObject's tag for differentiation.
    " ) ]
    public bool maintainPrefabDistribution = false;

    [Tooltip("Set newly unpooled items as the last child?")]
    public bool setAsLast = true;

    [Tooltip("Activate and deactivate unpooled and pooled items?")]
    public bool activateItems = true;

    [Tooltip("Activate and deactivate unpooled and pooled items?")]
    public bool updateDespawner = true;

    public List<GameObject> prefab;
    public List<GameObject> pooled;
    public List<GameObject> active;

    public virtual List<T> GetAllActive<T>() where T : Component
    {
        List<T> activeItems = new List<T>();

        foreach (var obj in this.active) {
            if (obj == null) continue;
            T item = obj.GetComponent<T>();
            if (item != null) activeItems.Add(item);
        }

        return activeItems;
    }

    public virtual T UnpoolItem<T>() where T : Component
    {
        T item = null;
        List<GameObject> available = null;

        // Make logic checks simpler in the future.
        if (prefab == null) prefab = new List<GameObject>();
        if (pooled == null) pooled = new List<GameObject>();

        // Here we filter our prefab list to contain only items of type T..
        List<GameObject> filteredPrefabs = prefab.FindAll(d => d.GetComponent<T>() != null);
        // ..and get a random sample from the filtered list.
        GameObject chosenPrefab = filteredPrefabs.Count <= 0 ? null :
            filteredPrefabs[Random.Range(0, filteredPrefabs.Count)];

        // If we are manitaining the prefab distribution, we need to grab
        //  only the pooled items that match our random sample.
        if (maintainPrefabDistribution)  {
            available = pooled.FindAll((d) => {
                return d.tag == chosenPrefab.tag && d.GetComponent<T>() != null;
            });
        }

        // Otherwise, we'll just grab all of the pooled items with matching componenets.
        else {
            available = pooled.FindAll(d => d.GetComponent<T>() != null);
        }

        // If there are any pooled items available,
        //  we'll grab one and remove it from the pool.
        if (available.Count > 0) {
            var obj = available[Random.Range(0, available.Count)];
            if (obj != null) {
                item = obj.GetComponent<T>();
                pooled.Remove(obj);
            }
        }

        // If up to this point, we have no item, we'll need to instantiate one.
        if (item == null && chosenPrefab != null) {
            item = Instantiate(chosenPrefab).GetComponent<T>();
            item.transform.SetParent(transform);
        }

        if (item == null)
        {
            Debug.LogWarning (
                string.Format (
                    "GenericGameObjectPool => No prefab containing a component of type \"{0}\".",
                    (typeof(T)).FullName
                ), this
            );

            return null;
        }

        if (active == null) active = new List<GameObject>(1);
        active.Add(item.gameObject);

        if (updateDespawner) {
            Despawner _d = item.GetComponent<Despawner>();
            if (_d) _d.pool = this;
        }

        if (activateItems) item.gameObject.SetActive(true);
        if (setAsLast) item.transform.SetAsLastSibling();

        return item;
    }

    public virtual void PoolItem<T>(T item) where T : Component {
        if (item == null) return;
        PoolItem(item.gameObject);
    }

    public virtual void PoolItem(GameObject item) {
        if (item == null) return;
        if (pooled == null) pooled = new List<GameObject>(1);
        if (!pooled.Contains(item)) pooled.Add(item);
        if (active != null) active.Remove(item);
        if (activateItems) item.SetActive(false);
    }

    public virtual void PoolAll() {
        for (int c = active.Count - 1; c >= 0; c--) {
            PoolItem(active[c]);
        }
    }
}
