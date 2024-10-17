function Main()
    -- When the main menu gets fired up.
end

function OnItemInteracted(name)
    -- When item picked up.
end

MainORF() -- ORF means "Override function"

--[[
    EXAMPLE USEAGE
]]

function MainORF()
    if isModsEnabled() then
        settingMenu.gameplay.AddOption("bool", "Name", "Desc", [0.5, 1, 0])
    end
end