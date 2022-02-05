local Shield = {} 	-- simple table for Shield. As there is only one, class not required
local Shared = require "shared"

Shield.barLength = 100
Shield.barHeight = 10
Shield.lineColour = Shared.WHITE
Shield.fillColour = Shared.DARKBLUE
Shield.pct = 100
Shield.outlineRect = { x = 5, y = 5, w = Shield.barLength, h = Shield.barHeight}
Shield.fillRect = { x = 5, y = 5, w = Shield.barLength, h = Shield.barHeight}

function Shield.update(value)
	if value < 0 then
		value = 0
	end	
	Shield.pct = value
	Shield.fillRect.w = (Shield.pct / 100) * Shield.barLength 
end

function Shield.draw()
	love.graphics.setColor(Shield.fillColour) 	-- fill background of whole rectangle
	love.graphics.rectangle("fill", Shield.fillRect.x, Shield.fillRect.y, Shield.barLength, Shield.barHeight)
	love.graphics.setColor(Shared.GREEN)				-- fill background of active shield value
	love.graphics.rectangle("fill", Shield.fillRect.x, Shield.fillRect.y, Shield.fillRect.w, Shield.fillRect.h)
	love.graphics.setColor(Shield.lineColour)	-- draw line round whole rectangle
	love.graphics.rectangle("line", Shield.outlineRect.x, Shield.outlineRect.y, Shield.outlineRect.w, Shield.outlineRect.h)
end

return Shield