﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEvahebLib.Enums
{
    public enum SET_TYPES // TODO where is this used?
    {
        SET_PARM1,
        SET_PARM2,
        SET_PARM3,
        SET_PARM4,
        SET_PARM5,
        SET_PARM6,
        SET_PARM7,
        SET_PARM8,
        SET_PARM9,
        SET_PARM10,
        SET_PARM11,
        SET_PARM12,
        SET_PARM13,
        SET_PARM14,
        SET_PARM15,
        SET_PARM16,

        //# #sep Scripts and other file paths
        SET_SPAWNSCRIPT,
        SET_USESCRIPT,
        SET_AWAKESCRIPT,
        SET_ANGERSCRIPT,
        SET_ATTACKSCRIPT,
        SET_VICTORYSCRIPT,
        SET_LOSTENEMYSCRIPT,
        SET_PAINSCRIPT,
        SET_FLEESCRIPT,
        SET_DEATHSCRIPT,
        SET_DELAYEDSCRIPT,
        SET_BLOCKEDSCRIPT,
        SET_FFIRESCRIPT,
        SET_FFDEATHSCRIPT,
        SET_MINDTRICKSCRIPT,
        SET_VIDEO_PLAY,
        SET_CINEMATIC_SKIPSCRIPT,
        SET_RAILCENTERTRACKLOCKED,
        SET_RAILCENTERTRACKUNLOCKED,
        SET_SKIN,

        //# #sep Standard strings
        SET_ENEMY,
        SET_LEADER,
        SET_NAVGOAL,
        SET_CAPTURE,
        SET_VIEWTARGET,
        SET_WATCHTARGET,
        SET_TARGETNAME,
        SET_PAINTARGET,
        SET_CAMERA_GROUP,
        SET_CAMERA_GROUP_TAG,
        SET_LOOK_TARGET,
        SET_ADDRHANDBOLT_MODEL,
        SET_REMOVERHANDBOLT_MODEL,
        SET_ADDLHANDBOLT_MODEL,
        SET_REMOVELHANDBOLT_MODEL,
        SET_CAPTIONTEXTCOLOR,
        SET_CENTERTEXTCOLOR,
        SET_SCROLLTEXTCOLOR,
        SET_COPY_ORIGIN,
        SET_DEFEND_TARGET,
        SET_TARGET,
        SET_TARGET2,
        SET_LOCATION,
        SET_REMOVE_TARGET,
        SET_LOADGAME,
        SET_LOCKYAW,
        SET_VIEWENTITY,
        SET_LOOPSOUND,
        SET_ICARUS_FREEZE,
        SET_ICARUS_UNFREEZE,
        SET_SABER1,
        SET_SABER2,
        SET_PLAYERMODEL,
        SET_VEHICLE,
        SET_SECURITY_KEY,

        SET_SCROLLTEXT,
        SET_LCARSTEXT,
        SET_CENTERTEXT,

        //# #sep vectors
        SET_ORIGIN,
        SET_ANGLES,
        SET_TELEPORT_DEST,
        SET_SABER_ORIGIN,

        //# #sep floats
        SET_XVELOCITY,
        SET_YVELOCITY,
        SET_ZVELOCITY,
        SET_Z_OFFSET,
        SET_DPITCH,
        SET_DYAW,
        SET_TIMESCALE,
        SET_CAMERA_GROUP_Z_OFS,
        SET_VISRANGE,
        SET_EARSHOT,
        SET_VIGILANCE,
        SET_GRAVITY,
        SET_FACEAUX,
        SET_FACEBLINK,
        SET_FACEBLINKFROWN,
        SET_FACEFROWN,
        SET_FACENORMAL,
        SET_FACEEYESCLOSED,
        SET_FACEEYESOPENED,
        SET_WAIT,
        SET_FOLLOWDIST,
        SET_SCALE,
        SET_RENDER_CULL_RADIUS,
        SET_DISTSQRD_TO_PLAYER,

        //# #sep ints
        SET_ANIM_HOLDTIME_LOWER,
        SET_ANIM_HOLDTIME_UPPER,
        SET_ANIM_HOLDTIME_BOTH,
        SET_HEALTH,
        SET_ARMOR,
        SET_WALKSPEED,
        SET_RUNSPEED,
        SET_YAWSPEED,
        SET_AGGRESSION,
        SET_AIM,
        SET_FRICTION,
        SET_SHOOTDIST,
        SET_HFOV,
        SET_VFOV,
        SET_DELAYSCRIPTTIME,
        SET_FORWARDMOVE,
        SET_RIGHTMOVE,
        SET_STARTFRAME,
        SET_ENDFRAME,
        SET_ANIMFRAME,
        SET_COUNT,
        SET_SHOT_SPACING,
        SET_MISSIONSTATUSTIME,
        SET_WIDTH,
        SET_SABER1BLADEON,
        SET_SABER1BLADEOFF,
        SET_SABER2BLADEON,
        SET_SABER2BLADEOFF,
        SET_DAMAGEENTITY,

        //# #sep booleans
        SET_IGNOREPAIN,
        SET_IGNOREENEMIES,
        SET_IGNOREALERTS,
        SET_DONTSHOOT,
        SET_NOTARGET,
        SET_DONTFIRE,
        SET_LOCKED_ENEMY,
        SET_CROUCHED,
        SET_WALKING,
        SET_RUNNING,
        SET_CHASE_ENEMIES,
        SET_LOOK_FOR_ENEMIES,
        SET_FACE_MOVE_DIR,
        SET_DONT_FLEE,
        SET_FORCED_MARCH,
        SET_UNDYING,
        SET_NOAVOID,
        SET_SOLID,
        SET_PLAYER_USABLE,
        SET_LOOP_ANIM,
        SET_INTERFACE,
        SET_SHIELDS,
        SET_INVISIBLE,
        SET_VAMPIRE,
        SET_FORCE_INVINCIBLE,
        SET_GREET_ALLIES,
        SET_VIDEO_FADE_IN,
        SET_VIDEO_FADE_OUT,
        SET_PLAYER_LOCKED,
        SET_LOCK_PLAYER_WEAPONS,
        SET_NO_IMPACT_DAMAGE,
        SET_NO_KNOCKBACK,
        SET_ALT_FIRE,
        SET_NO_RESPONSE,
        SET_INVINCIBLE,
        SET_MISSIONSTATUSACTIVE,
        SET_NO_COMBAT_TALK,
        SET_NO_ALERT_TALK,
        SET_TREASONED,
        SET_DISABLE_SHADER_ANIM,
        SET_SHADER_ANIM,
        SET_SABERACTIVE,
        SET_ADJUST_AREA_PORTALS,
        SET_DMG_BY_HEAVY_WEAP_ONLY,
        SET_SHIELDED,
        SET_NO_GROUPS,
        SET_FIRE_WEAPON,
        SET_FIRE_WEAPON_NO_ANIM,
        SET_SAFE_REMOVE,
        SET_BOBA_JET_PACK,
        SET_NO_MINDTRICK,
        SET_INACTIVE,
        SET_FUNC_USABLE_VISIBLE,
        SET_SECRET_AREA_FOUND,
        SET_END_SCREENDISSOLVE,
        SET_USE_CP_NEAREST,
        SET_MORELIGHT,
        SET_NO_FORCE,
        SET_NO_FALLTODEATH,
        SET_DISMEMBERABLE,
        SET_NO_ACROBATICS,
        SET_USE_SUBTITLES,
        SET_CLEAN_DAMAGING_ENTS,
        SET_HUD,
        //JKA
        SET_NO_PVS_CULL,
        SET_CLOAK,
        SET_FORCE_HEAL,
        SET_FORCE_SPEED,
        SET_FORCE_PUSH,
        SET_FORCE_PUSH_FAKE,
        SET_FORCE_PULL,
        SET_FORCE_MIND_TRICK,
        SET_FORCE_GRIP,
        SET_FORCE_LIGHTNING,
        SET_FORCE_SABERTHROW,
        SET_FORCE_RAGE,
        SET_FORCE_PROTECT,
        SET_FORCE_ABSORB,
        SET_FORCE_DRAIN,
        SET_WINTER_GEAR,
        SET_NO_ANGLES,

        //# #sep calls
        SET_SKILL,

        //# #sep Special tables
        SET_ANIM_UPPER,
        SET_ANIM_LOWER,
        SET_ANIM_BOTH,
        SET_PLAYER_TEAM,
        SET_ENEMY_TEAM,
        SET_BEHAVIOR_STATE,
        SET_DEFAULT_BSTATE,
        SET_TEMP_BSTATE,
        SET_EVENT,
        SET_WEAPON,
        SET_ITEM,
        SET_MUSIC_STATE,

        SET_FORCE_HEAL_LEVEL,
        SET_FORCE_JUMP_LEVEL,
        SET_FORCE_SPEED_LEVEL,
        SET_FORCE_PUSH_LEVEL,
        SET_FORCE_PULL_LEVEL,
        SET_FORCE_MINDTRICK_LEVEL,
        SET_FORCE_GRIP_LEVEL,
        SET_FORCE_LIGHTNING_LEVEL,
        SET_SABER_THROW,
        SET_SABER_DEFENSE,
        SET_SABER_OFFENSE,
        SET_FORCE_RAGE_LEVEL,
        SET_FORCE_PROTECT_LEVEL,
        SET_FORCE_ABSORB_LEVEL,
        SET_FORCE_DRAIN_LEVEL,
        SET_FORCE_SIGHT_LEVEL,
        SET_SABER1_COLOR1,
        SET_SABER1_COLOR2,
        SET_SABER2_COLOR1,
        SET_SABER2_COLOR2,
        SET_DISMEMBER_LIMB,

        SET_OBJECTIVE_SHOW,
        SET_OBJECTIVE_HIDE,
        SET_OBJECTIVE_SUCCEEDED,
        SET_OBJECTIVE_SUCCEEDED_NO_UPDATE,
        SET_OBJECTIVE_FAILED,

        SET_MISSIONFAILED,

        SET_TACTICAL_SHOW,
        SET_TACTICAL_HIDE,
        SET_OBJECTIVE_CLEARALL,

        //SET_OBJECTIVEFOSTER

        SET_OBJECTIVE_LIGHTSIDE,

        SET_MISSIONSTATUSTEXT,
        SET_MENU_SCREEN,

        SET_CLOSINGCREDITS,

        //in-bhc tables
        SET_LEAN,
    }
}
