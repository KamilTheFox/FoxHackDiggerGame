using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using GUIF;
using EntityType = BEDOMOKJJMN;

public class Entity : IHackMenu, IUpdate, IStarted, IGUIElement
{
    public string Name => nameof(Entity);
    private EntityType entityType = EntityType.ATOMBOMB;
    private float Scroll;
    private bool isSetEntity, isDNDEntity;
    private int idDND;
    private Vector3 positionEntity;
    private Vector3 EulerEntity;
    private int count = 500;
    private string newName = "name";
    private string Search = "";

    private int countClick;

    private bool isViewEntity, isDestroyEntity;

    private List<EntityType> FindListEntity = new List<EntityType>();

    private IEnumerable<EntityBase> FindEntityBase;

    private float Angle = 90F;
    private Dictionary<EntityType, string> TwoName = new Dictionary<EntityType, string>();

    public Action<int> Menu => (id) =>
    {
        BoxCast.ChildMenu("EntityMove", () =>
        {
            if (GUILayout.Button("Sellect: " + (isDNDEntity ? "On" : "Off"), null))
            {
                isDNDEntity = !isDNDEntity;
            }
            GUILayout.Box(idDND.ToString() , null);
            Angle = GUILayout.HorizontalSlider(Angle, 0, 180F, null);
            GUILayout.BeginHorizontal(null);
            if (GUILayout.Button("X+"+Angle.ToString("0"), null))
            {
                EulerEntity += Vector3.forward * Angle;
                SetNewRotate(EulerEntity);
            }
            if (GUILayout.Button("Y+"+Angle.ToString("0"), null))
            {
                EulerEntity += Vector3.up * Angle;
                SetNewRotate(EulerEntity);
            }
            if (GUILayout.Button("Z+"+Angle.ToString("0"), null))
            {
                EulerEntity += Vector3.left * Angle;
                SetNewRotate(EulerEntity);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(null);
            if (GUILayout.Button("X-"+Angle.ToString("0"), null))
            {
                EulerEntity -= Vector3.forward * Angle;
                SetNewRotate(EulerEntity);
            }
            if (GUILayout.Button("Y-" + Angle.ToString("0"), null))
            {
                EulerEntity -= Vector3.up * Angle;
                SetNewRotate(EulerEntity);
            }
            if (GUILayout.Button("Z-" + Angle.ToString("0"), null))
            {
                EulerEntity -= Vector3.left * Angle;
                SetNewRotate(EulerEntity);
            }
            GUILayout.EndHorizontal();
        });
        if(GUILayout.Button("GetEntity: " + (isSetEntity ? "On" : "Off"), null))
        {
            isSetEntity = !isSetEntity;
        }
        if(GUILayout.Button("isViewEntity: " + (isViewEntity? "On": "Off"), new GUILayoutOption[0]))
        {
            isViewEntity = !isViewEntity;
            FindEntityBase = GameObject.FindObjectsOfType<EntityBase>();
        }
        isDestroyEntity = GUILayout.Toggle(isDestroyEntity, "isDestroyEntity", null);
        if(countClick == 1)
            GUI.color = Color.red;
        if(GUILayout.Button("DeleteAllEntity", null))
        {
            FindEntityBase = GameObject.FindObjectsOfType<EntityBase>();
            if (countClick == 1)
            {
                foreach (EntityBase entity in FindEntityBase)
                {
                    BuildAPI.DestroyEntityRPC(entity);
                }
            }
            countClick++;
        }
        if(GUILayout.Button("DeleteFindsEntity", null))
        {
            FindEntityBase = GameObject.FindObjectsOfType<EntityBase>();
            if (countClick == 1)
            {
                foreach (EntityBase entity in FindEntityBase)
                {
                    if (FindListEntity.Contains(entity.BKFHOLFHJKE))
                    {
                        BuildAPI.DestroyEntityRPC(entity);
                    }
                }
            }
            countClick++;
        }
        GUI.color = SettingsMenu.ColorContent;
        GUILayout.Label("Select: " + GetName(entityType), null);
        newName = GUILayout.TextField(newName, null);
        if(GUILayout.Button("SetTwoName", null))
        {
            if(TwoName.ContainsKey(entityType))
            {
                TwoName[entityType] = newName;
            }
            else
                TwoName.Add(entityType, newName);
            SavingConfig.Save(SavingConfig.DirectoryType.Config, "Entity", TwoName);
        }
        Search = GUILayout.TextField(Search, null);
        GUILayout.BeginHorizontal(null);
        Scroll = GUILayout.VerticalSlider(Scroll, 0, count * 20, null);
        Rect area = GUILayoutUtility.GetRect(130, 250);
        GUI.Box(area, "");
        GUI.BeginGroup(area);
        int pos = 0;
        for (int i = 0; i < count; i++)
        {
            if (Search == "" || GetName((EntityType)i).ToLower().Contains(Search.ToLower()))
            {
                bool isContainsFind = false;
                if (isViewEntity && (isContainsFind = FindListEntity.Contains((EntityType)i)))
                {
                    GUI.color = Color.red;
                }
                if (GUI.Button(new Rect(5, 20 * pos - Scroll, area.width - 10, 20), GetName((EntityType)i)))
                {
                    if (isViewEntity)
                    {
                        if (isContainsFind)
                        {
                            FindListEntity.Remove((EntityType)i);
                        }
                        else
                            FindListEntity.Add((EntityType)i);
                    }
                    entityType = (EntityType)i;
                    newName = GetName((EntityType)i);
                }
                if(isViewEntity)
                {
                    GUI.color = SettingsMenu.ColorContent;
                }
                pos++;
            }
        }
        GUI.EndGroup();
        GUILayout.EndHorizontal();
        GUI.DragWindow();
    };
    private void SetNewRotate(Vector3 eulerRotate)
    {
        BuildAPI.MoveEntityRPC(idDND, positionEntity, Quaternion.Euler(eulerRotate));
    }
    private string GetName(EntityType type)
    {
        if(TwoName.TryGetValue(type, out string twoName))
        {
            return twoName;
        }
        return type.ToString();
    }
    public bool IsActive { get; set; }
    public Rect Rect { get; set; }
    private float time = 0F;
    public void Update()
    {
        if (countClick >= 1)
        {
            time += Time.deltaTime;
            if (time > 1.5F)
            {
                countClick = 0;
                time = 0F;
            }
        }
        if (RenameHash.isActiveMenu || RenameHash.isActiveChat)
            return;
        if(isDNDEntity || isDestroyEntity)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray ray = BuildAPI.GetRayInCamera();
                if (Physics.Raycast(ray, out RaycastHit hit, 999f, 1 << EntityBase.NEKNPHKOLIF | 1 << EntityBase.PGLJDGCMLIB | 1 << EntityBase.COEJBHHHCMF))
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        EntityBase entity = hit.collider.gameObject.GetComponentInParent<EntityBase>();
                        if (isDestroyEntity)
                        {
                            BuildAPI.DestroyEntityRPC(entity);
                        }
                        else
                        {
                            idDND = entity.photonView.viewIdField;
                            EulerEntity = entity.transform.rotation.eulerAngles;
                            positionEntity = entity.transform.position;
                        }
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = BuildAPI.GetRayInCamera();
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, 1 << LayerMask.NameToLayer("Terrain") | 1 << LayerMask.NameToLayer("Water")))
            {
                Vector3 vector = hit.point + hit.normal * 0.01f;
                MyIntVect vect = vector;
                if(isSetEntity)
                    BuildAPI.AddEntityRPC(entityType, vector, BuildAPI.EnityParametrs(entityType).gameObject.transform.rotation);
                if (isDNDEntity)
                    BuildAPI.MoveEntityRPC(idDND, vector, Quaternion.Euler(EulerEntity));
            }
        }
    }

    public void Start()
    {
        TwoName = SavingConfig.Open<Dictionary<EntityType, string>>(SavingConfig.DirectoryType.Config, "Entity");
    }

    public void OnGUI()
    {
        if (!isViewEntity || FindEntityBase == null) return;
        foreach(EntityBase entity in FindEntityBase)
        {
            if(FindListEntity.Contains(entity.BKFHOLFHJKE))
            {
                BuildFoxAPI.DrawStringToDislay(entity.transform.position, GetName(entity.BKFHOLFHJKE));
            }
        }

    }
}
