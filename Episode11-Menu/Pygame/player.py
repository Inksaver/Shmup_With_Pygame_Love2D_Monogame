import pygame
import shared, circle

class Player():
	def __init__(self, img, speed, scl):
		''' private class variables with no public access '''
		w, h = img.get_size()
		self.image:object      = pygame.transform.scale(img, (round(w * scl), round(h * scl)))
		self.image.set_colorkey(shared.BLACK)
		self.speedx:float      = speed
		self.scale:float       = scl
		self.power_timer:float = 0
		self.hide_timer:float  = 0
		''' private class variables with public getters/setters '''
		self._rect:object      = self.image.get_rect()	
		self._circle:object    = circle.Circle(0, 0, self._rect.height * 0.5)
		self._alive            = True
		self._shield:int       = 100
		self._lives:int        = 3
		self._hidden:bool      = False
		self._power:int        = 1
		self.reset(self._lives)
			
	@property
	def rect(self):
		return self._rect
	
	@property
	def circle(self):
		return self._circle	
	
	@property
	def lives(self):
		return self._lives
	
	@lives.setter
	def lives(self, value:int):
		self._lives = value
	
	@property
	def alive(self):
		return self._alive
	
	@alive.setter
	def alive(self, value:bool):
		self._alive	= value
		
	@property
	def shield(self):
		return self._shield
	
	@shield.setter
	def shield(self, value:int):
		self._shield = value
	
	@property
	def hidden(self):
		return self._hidden
	
	@property
	def power(self):
		return self._power
	
	def reset(self, lives):
		self._rect.centerx = round(shared.WIDTH * 0.5)
		self._rect.bottom = shared.HEIGHT - 10
		self._circle.center = self._rect.center	
		self._alive = True
		self._hidden = False	# reset flag and return player to normal position
		self._lives = lives
		self._power = 1
		
	''' methods '''
	def update(self, keystate, dt):
		''' update player position and hide / powerup timers '''
		
		if self._power > 1:
			self.power_timer += dt 			# timeout for powerups
			if self.power_timer >= 5:		# powerup running for 5 seconds
				self.powerdown()
	
		if self._hidden:					# timeout for hiding
			self.hide_timer += dt			# timeout for hiding
			if self.hide_timer > 2:			# hiding for 2 seconds
				self.unhide()
	
		if keystate[pygame.K_LEFT] or keystate[pygame.K_a]:
			self._rect.x -= round(self.speedx * dt)
		if keystate[pygame.K_RIGHT] or keystate[pygame.K_d]:
			self._rect.x += round(self.speedx * dt)
		''' Check if rectangle is out of bounds '''
		if self._rect.right > shared.WIDTH:
			self._rect.right = shared.WIDTH
		if self._rect.left < 0:
			self._rect.left = 0	
		self._circle.center = self._rect.center		# set circle position to match
	
	def draw(self) -> None:
		'''  draw player image (and debug rectangles) '''
		shared.screen.blit(self.image, self.rect)
		if shared.debug:
			pygame.draw.circle(shared.screen, shared.RED, self._circle.center, self._circle.radius, 1)
			pygame.draw.rect(shared.screen, shared.BLUE, self._rect, 1)		
	
	def hide(self) -> None:
		''' public method hide the player sprite by moving it off-screen '''
		self._hidden = True
		self.hide_timer = 0
		self._rect.center = (shared.WIDTH / 2, shared.HEIGHT + 200)
		self._circle.center = self._rect.center
		
	def unhide(self):
		''' private method stop hiding '''
		self.hide_timer = 0		# reset timer
		self.reset(self._lives)
		
	def powerup(self) -> None:
		''' public method increment power (2+ = 2 bullets can be fired '''
		self._power += 1
		self.power_timer = 0
		
	def powerdown(self):
		''' private method to return power to default '''
		self._power = 1				# return to 1 bullet
		self.power_timer = 0		# reset timer		
