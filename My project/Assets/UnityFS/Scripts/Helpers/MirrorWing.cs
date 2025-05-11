using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;

[AddComponentMenu("UnityFS/Helpers/Mirror Wing")]
[ExecuteInEditMode]
public class MirrorWing : MonoBehaviour 
{
#if UNITY_EDITOR	

	public Wing ParentWing;
	
	private Wing m_LocalWing;
	private ControlSurface m_LocalControlSurface;
	private PropWash m_LocalPropWash;
	private GroundEffect m_LocalGroundEffect;
	
	void Start () 
	{
		m_LocalWing = GetComponent<Wing>();
		m_LocalControlSurface = GetComponent<ControlSurface>();
		m_LocalPropWash = GetComponent<PropWash>();
		m_LocalGroundEffect = GetComponent<GroundEffect>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Make sure we have a parent wing before we try and do anything and also make sure they are similarly parented.
		if (ParentWing != null && transform.root.gameObject == ParentWing.transform.root.gameObject)
		{
			//Do not try and copy self.
			if (ParentWing == this)
			{
				return;
			}
			
			//Copy name ( only if changed )
			string newName = string.Format("{0}(Mirror)", ParentWing.name);
			if (gameObject.name != newName)
			{
				gameObject.name = newName;
			}
			
			//First copy transforms..
			transform.localScale = new Vector3( -ParentWing.transform.localScale.x, ParentWing.transform.localScale.y, ParentWing.transform.localScale.z );
			transform.localPosition = ParentWing.transform.localPosition;
			transform.localPosition = new Vector3( -transform.localPosition.x, transform.localPosition.y, transform.localPosition.z );
			
			transform.localRotation = new Quaternion( -ParentWing.transform.localRotation.x,
														ParentWing.transform.localRotation.y,
														ParentWing.transform.localRotation.z,
														ParentWing.transform.localRotation.w * -1.0f);
			
			//Copy wing
			if (m_LocalWing == null && ParentWing != null)
			{
				m_LocalWing = gameObject.AddComponent<Wing>();
			}
			if (m_LocalWing != null && ParentWing != null)
			{	
				m_LocalWing.SectionCount = ParentWing.SectionCount;
				m_LocalWing.WingTipWidthZeroToOne = ParentWing.WingTipWidthZeroToOne;
				m_LocalWing.WingTipSweep = ParentWing.WingTipSweep;
				m_LocalWing.WingTipAngle = ParentWing.WingTipAngle;
				m_LocalWing.Aerofoil = ParentWing.Aerofoil;
				m_LocalWing.CDOverride = ParentWing.CDOverride;
			}
			else if ( m_LocalWing != null && ParentWing == null )
			{
				//deleted on parent since copying.
				DestroyImmediate( m_LocalWing );
			}

			//Copy control surface
			ControlSurface parentControlSurface = ParentWing.GetComponent<ControlSurface>();
			if (m_LocalControlSurface == null && parentControlSurface != null)
			{
				m_LocalControlSurface = gameObject.AddComponent<ControlSurface>();
			}
			if (m_LocalControlSurface != null && parentControlSurface != null)
			{
				//string previousLocalAxisName = LocalControlSurface.AxisName;
				GameObject previousModel = m_LocalControlSurface.Model;
				Vector3 previousRotationAxis = m_LocalControlSurface.ModelRotationAxis;
				
				m_LocalControlSurface.MaxDeflectionDegrees = parentControlSurface.MaxDeflectionDegrees;
				m_LocalControlSurface.RootHingeDistanceFromTrailingEdge = parentControlSurface.RootHingeDistanceFromTrailingEdge;
				m_LocalControlSurface.TipHingeDistanceFromTrailingEdge = parentControlSurface.TipHingeDistanceFromTrailingEdge;
				m_LocalControlSurface.AffectedSections = parentControlSurface.AffectedSections;
				m_LocalControlSurface.Model = parentControlSurface.Model;
				m_LocalControlSurface.ModelRotationAxis = parentControlSurface.ModelRotationAxis;
				 
				//Keep the following unique.
				m_LocalControlSurface.Model = previousModel;
				m_LocalControlSurface.ModelRotationAxis = previousRotationAxis;
			}
			else if ( m_LocalControlSurface != null && parentControlSurface == null)
			{
				//deleted on parent since copying.
				DestroyImmediate( m_LocalControlSurface );
			}
				
			//Copy propwash 
			PropWash parentPropWash = ParentWing.GetComponent<PropWash>();
			if (m_LocalPropWash == null && parentPropWash != null)
			{
				m_LocalPropWash = gameObject.AddComponent<PropWash>();
			}
			if ( m_LocalPropWash != null && parentPropWash != null)
			{
				Engine previousPropwashSource = m_LocalPropWash.PropWashSource;
				
				//Copy everything except propwash source..
				m_LocalPropWash.AffectedSections = parentPropWash.AffectedSections;
				m_LocalPropWash.PropWashSource = parentPropWash.PropWashSource;
				m_LocalPropWash.PropWashStrength = parentPropWash.PropWashStrength;
				
				//Keep unique
				m_LocalPropWash.PropWashSource = previousPropwashSource;
			}
			else if ( m_LocalPropWash != null && parentPropWash == null)
			{
				//deleted on parent since copying.
				DestroyImmediate( m_LocalPropWash );
			}
			
			//Copy groundeffect.
			GroundEffect parentGroundEffect = ParentWing.GetComponent<GroundEffect>();
			if (m_LocalGroundEffect == null && parentGroundEffect != null)
			{
				m_LocalGroundEffect = gameObject.AddComponent<GroundEffect>();
			}
			if ( m_LocalGroundEffect != null && parentGroundEffect != null)
			{
				//Copy everything..
				m_LocalGroundEffect.CLHeightVsChord = parentGroundEffect.CLHeightVsChord;
				m_LocalGroundEffect.CDHeightVsSpan = parentGroundEffect.CDHeightVsSpan;
				m_LocalGroundEffect.RayCastAxis = parentGroundEffect.RayCastAxis;
				m_LocalGroundEffect.RayCastLayers = parentGroundEffect.RayCastLayers;
				m_LocalGroundEffect.Wingspan = parentGroundEffect.Wingspan;
			}
			else if ( m_LocalGroundEffect != null && parentGroundEffect == null)
			{
				//deleted on parent since copying.
				DestroyImmediate( m_LocalGroundEffect );
			}
		}
	}
#endif

}
