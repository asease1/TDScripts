using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public static class XmlConverter{

	public static List<MonsterStone> convertXmlToMonsterStone(string xmlPath)
    {
        XmlDocument document = new XmlDocument();
        document.Load(xmlPath);

        XmlNodeList nodes = document.SelectNodes("/waveList")[0].ChildNodes;

        List<MonsterStone> resualt = new List<MonsterStone>();
        for(int i = 0; i < nodes.Count; i++)
        {
            int timer = 0;
            MonsterStone.MonsterType type = MonsterStone.MonsterType.Harpy;
            List<MonsterStone.MonsterAbility> abilities = null;
            for(int j = 0; j < nodes[i].ChildNodes.Count; j++)
            {
                switch (nodes[i].ChildNodes[j].Name.ToLower())
                {
               
                    case "timer":
                        int.TryParse( nodes[i].ChildNodes[j].InnerText, out timer);
                        break;
                    case "type":
                        type = MonsterStone.ParseStringToType(nodes[i].ChildNodes[j].InnerText);
                        break;
                    case "abilities":
                        abilities = getAbilities(nodes[i].ChildNodes[j].ChildNodes);
                        break;
                }
            }

            resualt.Add(new MonsterStone(type, timer, abilities));
        }

        return resualt;
    }

    private static List<MonsterStone.MonsterAbility> getAbilities(XmlNodeList list)
    {
        List<MonsterStone.MonsterAbility> resualt = new List<MonsterStone.MonsterAbility>();
        for(int i = 0; i < list.Count; i++)
        {
            resualt.Add(MonsterStone.getAbility(list[i].InnerText));
        }
        return resualt;
    }
}
