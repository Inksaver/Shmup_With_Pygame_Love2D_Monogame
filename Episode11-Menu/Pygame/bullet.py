import pygame
import shared, circle

class Bullet():
	def __init__(self, img:object, player:object, align:str = "centre") -> None:
		''' Class variables. Those with getters/setters are indicated with _ '''
		self.image = img							# assign image to class variable
		self.image.set_colorkey(shared.BLACK)		# make image background transparent		
		self._rect = self.image.get_rect() 			# filled rectangle as taken from image
		self._rect.bottom = player.rect.top			# bottom of bullet at top of player
		self._rect.centerx = player.rect.centerx	# align bullet with centre of player by default
		if align == "left":							# bullet from left of player
			self._rect.left = player.rect.left
		elif align == "right":						# bullet from right of player
			self._rect.right = player.rect.right
		self.speedy = 1000
		if shared.debug:							# reduce speed in debug mode
			self.speedy = self.speedy / 2
		self.radius = min(self._rect.width, self._rect.height) / 2	# radius is half the smallest of width/height
		self._circle = circle.Circle(self._rect.centerx, self._rect.centery, self.radius)				
		self.active = True							# False when off-screen
		
	@property
	def rect(self):									# use .rect instead of .get_rect()
		return self._rect

	@property										# use .circle instead of .get_circle()
	def circle(self):	
		return self._circle

	def update(self, dt) -> bool:
		self._rect.y -= round(self.speedy * dt)		# move bullet up the window
		self._circle.y = self._rect.y				# match circle collider to rect
		# delete if moves off top of screen
		if self._rect.bottom < 0:
			self.active = False

		return self.active

	def draw(self) -> None:
		shared.screen.blit(self.image, self._rect)	# draw image
		if shared.debug:							# draw colliders
			pygame.draw.circle(shared.screen, shared.RED, self._circle.center, self._circle.radius, 1)
			pygame.draw.rect(shared.screen, shared.BLUE, self._rect, 1)	