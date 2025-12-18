using System;

[Serializable]
public class UserData
{
    public string name;
    public string email;
    public string avatar;
    public int stars;
    public string character;

    public UserData()
    {
        name = "";
        email = "";
        avatar = "default_avatar";
        stars = 0;
        character = "default_character";
    }

    public UserData(string name, string email)
    {
        this.name = name;
        this.email = email;
        this.avatar = "default_avatar";
        this.stars = 0;
        this.character = "default_character";
    }
}